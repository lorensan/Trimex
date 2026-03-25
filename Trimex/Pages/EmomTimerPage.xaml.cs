using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class EmomTimerPage : ContentPage
{
    private const string PlayIcon = "play.png";
    private const string PauseIcon = "pause.png";

    private readonly EmomConfigurationRequest _configuration;
    private readonly IDispatcherTimer _timer;

    private WorkoutTimerState _state = WorkoutTimerState.Idle;
    private DateTimeOffset? _preCountdownStartedAtUtc;
    private DateTimeOffset? _currentRunStartedAtUtc;
    private TimeSpan _elapsedBeforeCurrentRun = TimeSpan.Zero;
    private int _currentRound = 1;
    private int _lastPreCountdownCueSecond = int.MaxValue;
    private int _lastFinalWarningSecond = int.MaxValue;

    public EmomTimerPage(EmomConfigurationRequest configuration)
    {
        InitializeComponent();

        _configuration = configuration;

        WorkoutTitleLabel.Text = "EMOM";
        WorkoutContextLabel.Text = BuildContextLabel(configuration);

        ProgressRing.AccentColor = Color.FromArgb("#FFD166");

        ExerciseLabel.IsVisible = !string.IsNullOrWhiteSpace(configuration.WodDescription);
        ExerciseLabel.Text = configuration.WodDescription;

        RoundLabel.Text = $"Round 1 of {configuration.Rounds}";

        _timer = Dispatcher.CreateTimer();
        _timer.Interval = TimeSpan.FromMilliseconds(200);
        _timer.Tick += OnTimerTick;

        UpdateVisualState();
    }

    protected override void OnDisappearing()
    {
        _timer.Stop();
        base.OnDisappearing();
    }

    private void OnTimerActionClicked(object? sender, EventArgs e)
    {
        switch (_state)
        {
            case WorkoutTimerState.Idle:
                StartPreCountdown();
                break;
            case WorkoutTimerState.Running:
                PauseWorkout();
                break;
            case WorkoutTimerState.Paused:
                ResumeWorkout();
                break;
        }
    }

    private void OnStateTapped(object? sender, TappedEventArgs e)
    {
        switch (_state)
        {
            case WorkoutTimerState.PreCountdown:
                CancelPreCountdown();
                break;
            case WorkoutTimerState.Running:
                PauseWorkout();
                break;
            case WorkoutTimerState.Paused:
                ResumeWorkout();
                break;
        }
    }

    private async void OnEndClicked(object? sender, EventArgs e)
    {
        var shouldEnd = await DisplayAlertAsync("End workout", "Do you want to stop the current workout and go back?", "Yes", "No");

        if (!shouldEnd)
        {
            return;
        }

        _timer.Stop();
        await Navigation.PopAsync();
    }

    private void OnTimerTick(object? sender, EventArgs e)
    {
        switch (_state)
        {
            case WorkoutTimerState.PreCountdown:
                HandlePreCountdownTick();
                break;
            case WorkoutTimerState.Running:
                HandleRunningTick();
                break;
        }
    }

    private void StartPreCountdown()
    {
        ResetCueTracking();
        _state = WorkoutTimerState.PreCountdown;
        _preCountdownStartedAtUtc = DateTimeOffset.UtcNow;
        _timer.Start();
        UpdateVisualState();
    }

    private void CancelPreCountdown()
    {
        _timer.Stop();
        _preCountdownStartedAtUtc = null;
        _state = WorkoutTimerState.Idle;
        ResetCueTracking();
        UpdateVisualState();
    }

    private void PauseWorkout()
    {
        if (_currentRunStartedAtUtc is not null)
        {
            _elapsedBeforeCurrentRun += DateTimeOffset.UtcNow - _currentRunStartedAtUtc.Value;
            _currentRunStartedAtUtc = null;
        }

        _timer.Stop();
        _state = WorkoutTimerState.Paused;
        UpdateVisualState();
    }

    private void ResumeWorkout()
    {
        _state = WorkoutTimerState.Running;
        _currentRunStartedAtUtc = DateTimeOffset.UtcNow;
        _timer.Start();
        UpdateVisualState();
    }

    private void HandlePreCountdownTick()
    {
        if (_preCountdownStartedAtUtc is null)
        {
            return;
        }

        var elapsed = DateTimeOffset.UtcNow - _preCountdownStartedAtUtc.Value;
        var remaining = 10 - (int)Math.Floor(elapsed.TotalSeconds);

        if (remaining <= 0)
        {
            _state = WorkoutTimerState.Running;
            _preCountdownStartedAtUtc = null;
            _currentRunStartedAtUtc = DateTimeOffset.UtcNow;
            _currentRound = 1;
            ProgressRing.Progress = 0;
            _ = TimerCueService.PlayStartSequenceAsync();
            UpdateVisualState();
            return;
        }

        TryPlayPreCountdownCue(remaining);
        StateValueLabel.Text = remaining.ToString();
        StateHintLabel.Text = "Tap to cancel";
        PauseActionButton.IsVisible = false;
    }

    private void HandleRunningTick()
    {
        var totalElapsed = GetElapsed();
        var intervalSeconds = _configuration.IntervalSeconds;
        var completedRounds = (int)(totalElapsed.TotalSeconds / intervalSeconds);
        var currentRound = completedRounds + 1;

        if (completedRounds >= _configuration.Rounds)
        {
            CompleteWorkout();
            return;
        }

        // Detect round transition — play start beep for each new round (skip round 1, already played on pre-countdown end)
        if (currentRound != _currentRound)
        {
            _currentRound = currentRound;
            _lastFinalWarningSecond = int.MaxValue;
            _ = TimerCueService.PlayStartSequenceAsync();
        }

        var elapsedInInterval = totalElapsed.TotalSeconds - completedRounds * intervalSeconds;
        var remainingInInterval = TimeSpan.FromSeconds(intervalSeconds - elapsedInInterval);

        TryPlayFinalWarningCue(remainingInInterval);

        RoundLabel.Text = $"Round {_currentRound} of {_configuration.Rounds}";
        StateValueLabel.Text = FormatClock(TimeSpan.FromSeconds(elapsedInInterval));
        StateHintLabel.Text = "Tap the time or pause";
        PauseActionButton.IsVisible = true;
        ProgressRing.Progress = elapsedInInterval / intervalSeconds;
    }

    private void CompleteWorkout()
    {
        _timer.Stop();
        _state = WorkoutTimerState.Completed;
        _currentRunStartedAtUtc = null;
        _ = TimerCueService.PlayCompletionSequenceAsync();
        UpdateVisualState();
        _ = DisplayAlertAsync("Workout completed", $"All {_configuration.Rounds} rounds done! Great work.", "OK");
    }

    private void TryPlayPreCountdownCue(int remainingSeconds)
    {
        if (remainingSeconds is < 1 or > 3 || remainingSeconds == _lastPreCountdownCueSecond)
        {
            return;
        }

        _lastPreCountdownCueSecond = remainingSeconds;
        _ = TimerCueService.PlayCountdownWarningAsync();
    }

    private void TryPlayFinalWarningCue(TimeSpan remaining)
    {
        var remainingSeconds = (int)Math.Ceiling(remaining.TotalSeconds);

        if (remainingSeconds is < 1 or > 3 || remainingSeconds == _lastFinalWarningSecond)
        {
            return;
        }

        _lastFinalWarningSecond = remainingSeconds;
        _ = TimerCueService.PlayCountdownWarningAsync();
    }

    private void ResetCueTracking()
    {
        _lastPreCountdownCueSecond = int.MaxValue;
        _lastFinalWarningSecond = int.MaxValue;
    }

    private TimeSpan GetElapsed() =>
        _elapsedBeforeCurrentRun +
        (_currentRunStartedAtUtc is null
            ? TimeSpan.Zero
            : DateTimeOffset.UtcNow - _currentRunStartedAtUtc.Value);

    private void UpdateVisualState()
    {
        TimerActionButton.Source = PlayIcon;
        PauseActionButton.Source = PauseIcon;
        PauseActionButton.IsVisible = false;

        switch (_state)
        {
            case WorkoutTimerState.Idle:
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = string.Empty;
                ProgressRing.Progress = 0;
                TimerActionButton.IsEnabled = true;
                TimerActionButton.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                RoundLabel.Text = $"Round 1 of {_configuration.Rounds}";
                break;

            case WorkoutTimerState.PreCountdown:
                StateValueLabel.Text = "10";
                StateHintLabel.Text = "Tap to cancel";
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;

            case WorkoutTimerState.Running:
                StateHintLabel.Text = "Tap the time or pause";
                PauseActionButton.IsVisible = true;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;

            case WorkoutTimerState.Paused:
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = string.Empty;
                TimerActionButton.IsEnabled = true;
                TimerActionButton.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                RoundLabel.Text = $"Round {_currentRound} of {_configuration.Rounds}";
                break;

            case WorkoutTimerState.Completed:
                StateValueLabel.Text = "Done!";
                StateHintLabel.Text = "Workout completed.";
                ProgressRing.Progress = 1;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                RoundLabel.Text = $"Round {_configuration.Rounds} of {_configuration.Rounds}";
                break;
        }
    }

    private static string BuildContextLabel(EmomConfigurationRequest configuration)
    {
        var intervalLabel = EmomConfigurationPage.FormatDurationLabel(configuration.IntervalSeconds);
        return $"{intervalLabel}  ×  {configuration.Rounds} rounds";
    }

    private static string FormatClock(TimeSpan value)
    {
        var totalMinutes = Math.Max(0, (int)value.TotalMinutes);
        return $"{totalMinutes:00}:{value.Seconds:00}";
    }

    private enum WorkoutTimerState
    {
        Idle,
        PreCountdown,
        Running,
        Paused,
        Completed
    }
}
