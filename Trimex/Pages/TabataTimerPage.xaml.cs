using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class TabataTimerPage : ContentPage
{
    private const string PlayIcon = "play.png";
    private const string PauseIcon = "pause.png";

    private static readonly Color WorkColor = Color.FromArgb("#35D07F");
    private static readonly Color RestColor = Color.FromArgb("#FF3B3B");

    private readonly TabataConfigurationRequest _configuration;
    private readonly IDispatcherTimer _timer;

    private WorkoutTimerState _state = WorkoutTimerState.Idle;
    private DateTimeOffset? _preCountdownStartedAtUtc;
    private DateTimeOffset? _currentRunStartedAtUtc;
    private TimeSpan _elapsedBeforeCurrentRun = TimeSpan.Zero;
    private int _currentDisplayRound = 1;
    private bool _currentPhaseIsWork = true;
    private int _lastPreCountdownCueSecond = int.MaxValue;
    private int _lastFinalWarningSecond = int.MaxValue;

    public TabataTimerPage(TabataConfigurationRequest configuration)
    {
        InitializeComponent();

        _configuration = configuration;

        WorkoutContextLabel.Text = BuildContextLabel(configuration);

        ExerciseLabel.IsVisible = !string.IsNullOrWhiteSpace(configuration.WodDescription);
        ExerciseLabel.Text = configuration.WodDescription;

        ProgressRing.AccentColor = WorkColor;

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
        if (_preCountdownStartedAtUtc is null) return;

        var elapsed = DateTimeOffset.UtcNow - _preCountdownStartedAtUtc.Value;
        var remaining = 10 - (int)Math.Floor(elapsed.TotalSeconds);

        if (remaining <= 0)
        {
            _state = WorkoutTimerState.Running;
            _preCountdownStartedAtUtc = null;
            _currentRunStartedAtUtc = DateTimeOffset.UtcNow;
            _currentDisplayRound = 1;
            _currentPhaseIsWork = true;
            ProgressRing.Progress = 0;
            _ = TimerCueService.PlayStartSequenceAsync();
            UpdateVisualState();
            return;
        }

        TryPlayPreCountdownCue(remaining);
        StateValueLabel.Text = remaining.ToString();
        StateValueLabel.TextColor = Colors.White;
        StateHintLabel.Text = "Tap to cancel";
        PauseActionButton.IsVisible = false;
    }

    private void HandleRunningTick()
    {
        var totalElapsedSecs = GetElapsed().TotalSeconds;
        var workSecs = _configuration.WorkSeconds;
        var restSecs = _configuration.RestSeconds;
        var totalRounds = _configuration.Rounds;

        var phaseStart = 0.0;
        for (var round = 0; round < totalRounds; round++)
        {
            // Work phase
            if (totalElapsedSecs < phaseStart + workSecs)
            {
                UpdateWorkPhase(round + 1, totalElapsedSecs - phaseStart, workSecs, totalRounds);
                return;
            }
            phaseStart += workSecs;

            // Rest phase — only between rounds (not after the last one)
            if (round < totalRounds - 1)
            {
                if (totalElapsedSecs < phaseStart + restSecs)
                {
                    UpdateRestPhase(round + 1, totalElapsedSecs - phaseStart, restSecs, totalRounds);
                    return;
                }
                phaseStart += restSecs;
            }
        }

        CompleteWorkout();
    }

    private void UpdateWorkPhase(int round, double elapsedInPhase, int phaseDuration, int totalRounds)
    {
        // Detect transition into a new work phase
        if (!_currentPhaseIsWork || _currentDisplayRound != round)
        {
            _currentPhaseIsWork = true;
            _currentDisplayRound = round;
            _lastFinalWarningSecond = int.MaxValue;
            // Round 1 already got the start beep from pre-countdown
            if (round > 1) _ = TimerCueService.PlayStartSequenceAsync();
        }

        var remaining = TimeSpan.FromSeconds(phaseDuration - elapsedInPhase);
        TryPlayFinalWarningCue(remaining);

        RoundLabel.Text = $"Round {round} / {totalRounds}";
        RoundLabel.TextColor = WorkColor;
        PhaseLabel.Text = "WORK";
        PhaseLabel.TextColor = WorkColor;
        StateValueLabel.TextColor = WorkColor;
        StateValueLabel.Text = FormatClock(TimeSpan.FromSeconds(elapsedInPhase));
        ProgressRing.AccentColor = WorkColor;
        ProgressRing.Progress = elapsedInPhase / phaseDuration;
        StateHintLabel.Text = "Tap to pause";
        PauseActionButton.IsVisible = true;
    }

    private void UpdateRestPhase(int completedRound, double elapsedInPhase, int phaseDuration, int totalRounds)
    {
        // Detect transition into rest phase
        if (_currentPhaseIsWork)
        {
            _currentPhaseIsWork = false;
            _currentDisplayRound = completedRound;
            _lastFinalWarningSecond = int.MaxValue;
            _ = TimerCueService.PlayCountdownWarningAsync();
        }

        var remaining = TimeSpan.FromSeconds(phaseDuration - elapsedInPhase);
        TryPlayFinalWarningCue(remaining);

        RoundLabel.Text = $"Round {completedRound} / {totalRounds}";
        RoundLabel.TextColor = RestColor;
        PhaseLabel.Text = "REST";
        PhaseLabel.TextColor = RestColor;
        StateValueLabel.TextColor = RestColor;
        StateValueLabel.Text = FormatClock(TimeSpan.FromSeconds(elapsedInPhase));
        ProgressRing.AccentColor = RestColor;
        ProgressRing.Progress = elapsedInPhase / phaseDuration;
        StateHintLabel.Text = "Tap to pause";
        PauseActionButton.IsVisible = true;
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
        if (remainingSeconds is < 1 or > 3 || remainingSeconds == _lastPreCountdownCueSecond) return;
        _lastPreCountdownCueSecond = remainingSeconds;
        _ = TimerCueService.PlayCountdownWarningAsync();
    }

    private void TryPlayFinalWarningCue(TimeSpan remaining)
    {
        var remainingSeconds = (int)Math.Ceiling(remaining.TotalSeconds);
        if (remainingSeconds is < 1 or > 3 || remainingSeconds == _lastFinalWarningSecond) return;
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
                RoundLabel.Text = $"Round 1 / {_configuration.Rounds}";
                RoundLabel.TextColor = WorkColor;
                PhaseLabel.Text = string.Empty;
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = string.Empty;
                ProgressRing.Progress = 0;
                ProgressRing.AccentColor = WorkColor;
                TimerActionButton.IsEnabled = true;
                TimerActionButton.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                break;

            case WorkoutTimerState.PreCountdown:
                StateValueLabel.Text = "10";
                StateValueLabel.TextColor = Colors.White;
                StateHintLabel.Text = "Tap to cancel";
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;

            case WorkoutTimerState.Running:
                StateHintLabel.Text = "Tap to pause";
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
                RoundLabel.Text = $"Round {_currentDisplayRound} / {_configuration.Rounds}";
                break;

            case WorkoutTimerState.Completed:
                RoundLabel.Text = $"Round {_configuration.Rounds} / {_configuration.Rounds}";
                RoundLabel.TextColor = WorkColor;
                PhaseLabel.Text = "DONE";
                PhaseLabel.TextColor = WorkColor;
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = "Slide to go back.";
                ProgressRing.Progress = 1;
                ProgressRing.AccentColor = WorkColor;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;
        }
    }

    private static string BuildContextLabel(TabataConfigurationRequest configuration)
    {
        var workLabel = TabataConfigurationPage.FormatDurationLabel(configuration.WorkSeconds);
        var restLabel = TabataConfigurationPage.FormatDurationLabel(configuration.RestSeconds);
        return $"{workLabel} work  /  {restLabel} rest  ×  {configuration.Rounds} rounds";
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
