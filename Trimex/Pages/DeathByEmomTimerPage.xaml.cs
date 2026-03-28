using Trimex.Services;

namespace Trimex.Pages;

public partial class DeathByEmomTimerPage : ContentPage
{
    private const string PlayIcon = "play.png";
    private const string PauseIcon = "pause.png";

    private readonly int _intervalSeconds;
    private readonly IDispatcherTimer _timer;

    private WorkoutTimerState _state = WorkoutTimerState.Idle;
    private DateTimeOffset? _preCountdownStartedAtUtc;
    private DateTimeOffset? _currentRunStartedAtUtc;
    private TimeSpan _elapsedBeforeCurrentRun = TimeSpan.Zero;
    private int _currentRound = 1;
    private int _lastPreCountdownCueSecond = int.MaxValue;
    private int _lastFinalWarningSecond = int.MaxValue;

    public DeathByEmomTimerPage(int intervalSeconds)
    {
        InitializeComponent();

        _intervalSeconds = intervalSeconds;

        WorkoutTitleLabel.Text = "Death by EMOM";
        WorkoutContextLabel.Text = $"{EmomConfigurationPage.FormatDurationLabel(intervalSeconds)} each";

        ProgressRing.AccentColor = Color.FromArgb("#FF3B3B");

        RoundLabel.Text = "Round 1";

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

    private void OnSlideCompleted(object? sender, EventArgs e)
    {
        _timer.Stop();
        _state = WorkoutTimerState.Completed;
        _currentRunStartedAtUtc = null;

        var completedRounds = _currentRound - 1;
        var totalElapsed = _elapsedBeforeCurrentRun;
        var totalMinutes = (int)totalElapsed.TotalMinutes;
        var totalFormatted = $"{totalMinutes:00}:{(int)totalElapsed.TotalSeconds % 60:00}";

        WorkoutTitleLabel.Text = $"Round {completedRounds} in {totalFormatted} minutes";
        WorkoutContextLabel.Text = "Session ended. Great work!";
        _ = TimerCueService.PlayCompletionSequenceAsync();
        UpdateVisualState();
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
        var completedIntervals = (int)(totalElapsed.TotalSeconds / _intervalSeconds);
        var currentRound = completedIntervals + 1;

        // Detect interval auto-reset — beep and start new round
        if (currentRound != _currentRound)
        {
            _currentRound = currentRound;
            _lastFinalWarningSecond = int.MaxValue;
            _ = TimerCueService.PlayStartSequenceAsync();
        }

        var elapsedInInterval = totalElapsed.TotalSeconds - completedIntervals * _intervalSeconds;
        var remaining = TimeSpan.FromSeconds(_intervalSeconds - elapsedInInterval);

        TryPlayFinalWarningCue(remaining);

        RoundLabel.Text = $"Round {_currentRound}";
        StateValueLabel.Text = FormatClock(remaining);
        StateHintLabel.Text = "Tap to pause";
        PauseActionButton.IsVisible = true;
        ProgressRing.Progress = elapsedInInterval / _intervalSeconds;
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
                TimerActionImage.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                RoundLabel.Text = "Round 1";
                break;

            case WorkoutTimerState.PreCountdown:
                StateValueLabel.Text = "10";
                StateHintLabel.Text = "Tap to cancel";
                TimerActionButton.IsVisible = false;
                TimerActionImage.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;

            case WorkoutTimerState.Running:
                StateHintLabel.Text = "Tap to pause";
                PauseActionButton.IsVisible = true;
                TimerActionButton.IsVisible = false;
                TimerActionImage.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;

            case WorkoutTimerState.Paused:
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = string.Empty;
                TimerActionButton.IsEnabled = true;
                TimerActionButton.IsVisible = true;
                TimerActionImage.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                break;

            case WorkoutTimerState.Completed:
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = "Slide to go back.";
                TimerActionButton.IsVisible = false;
                TimerActionImage.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;
        }
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
