using System.Text;
using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class WorkoutTimerPage : ContentPage
{
    private const string PlayIcon = "play.png";
    private const string PauseIcon = "pause.png";

    private readonly WorkoutConfigurationRequest _configuration;
    private readonly IDispatcherTimer _timer;

    private WorkoutTimerState _state = WorkoutTimerState.Idle;
    private DateTimeOffset? _preCountdownStartedAtUtc;
    private DateTimeOffset? _currentRunStartedAtUtc;
    private TimeSpan _elapsedBeforeCurrentRun = TimeSpan.Zero;
    private int _roundCount;
    private int _lastPreCountdownCueSecond = int.MaxValue;
    private int _lastFinalWarningSecond = int.MaxValue;

    public WorkoutTimerPage(WorkoutConfigurationRequest configuration)
    {
        InitializeComponent();

        _configuration = configuration;
        Title = configuration.TypeDisplayName;

        WorkoutTitleLabel.Text = configuration.TypeDisplayName;
        WorkoutContextLabel.Text = BuildContextLabel(configuration);
        WorkoutMinutesWatermarkLabel.Text = BuildWatermarkMinutes(configuration);
        var workoutDetails = BuildWorkoutDetails(configuration);
        WorkoutDetailsLabel.Text = workoutDetails;
        DetailsButton.IsVisible = !string.IsNullOrWhiteSpace(workoutDetails);
        WorkoutDetailsContainer.IsVisible = false;

        RoundSection.IsVisible = configuration.SupportsRounds;
        RoundButton.Text = "00";

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
            case WorkoutTimerState.PreCountdown:
                break;
            case WorkoutTimerState.Running:
                PauseWorkout();
                break;
            case WorkoutTimerState.Paused:
                ResumeWorkout();
                break;
            case WorkoutTimerState.Completed:
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
        if (_state is WorkoutTimerState.Idle or WorkoutTimerState.Completed)
        {
            _timer.Stop();
            await Navigation.PopAsync();
            return;
        }

        var shouldEnd = await DisplayAlertAsync("End workout", "Do you want to stop the current workout and go back?", "Yes", "No");

        if (!shouldEnd)
        {
            return;
        }

        _timer.Stop();
        await Navigation.PopAsync();
    }

    private void OnDetailsClicked(object? sender, EventArgs e)
    {
        if (!DetailsButton.IsVisible)
        {
            return;
        }

        WorkoutDetailsContainer.IsVisible = !WorkoutDetailsContainer.IsVisible;
    }

    private async void OnRoundClicked(object? sender, EventArgs e)
    {
        _roundCount++;
        RoundButton.Text = _roundCount.ToString("00");
        await PlayConfettiAsync();
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
            ProgressRing.Progress = 0;
            _ = TimerCueService.PlayStartSequenceAsync();
            UpdateVisualState();
            return;
        }

        TryPlayPreCountdownCue(remaining);

        StateValueLabel.Text = remaining.ToString();
        StateHintLabel.Text = "Tap to cancel";
        PausedTimeLabel.IsVisible = false;
        PauseActionButton.IsVisible = false;
    }

    private void HandleRunningTick()
    {
        var elapsed = GetElapsed();

        if (_configuration.CountsDown)
        {
            var remaining = TimeSpan.FromSeconds(_configuration.DurationSeconds) - elapsed;

            if (remaining <= TimeSpan.Zero)
            {
                CompleteWorkout(TimeSpan.Zero);
                return;
            }

            TryPlayFinalWarningCue(remaining);
            StateValueLabel.Text = FormatClock(remaining);
            StateHintLabel.Text = "Tap the time or pause";
            ProgressRing.Progress = elapsed.TotalSeconds / _configuration.DurationSeconds;
            PausedTimeLabel.IsVisible = false;

            return;
        }

        StateValueLabel.Text = FormatClock(elapsed);
        StateHintLabel.Text = "Tap the time or pause";
        PausedTimeLabel.IsVisible = false;

        if (_configuration.TimeCapSeconds is int timeCapSeconds && timeCapSeconds > 0)
        {
            TryPlayFinalWarningCue(TimeSpan.FromSeconds(timeCapSeconds) - elapsed);
            ProgressRing.Progress = Math.Min(1d, elapsed.TotalSeconds / timeCapSeconds);

            if (elapsed.TotalSeconds >= timeCapSeconds)
            {
                CompleteWorkout(TimeSpan.FromSeconds(timeCapSeconds));
            }

            return;
        }

        ProgressRing.Progress = 0;
    }

    private void CompleteWorkout(TimeSpan finalTime)
    {
        _timer.Stop();
        _state = WorkoutTimerState.Completed;
        _currentRunStartedAtUtc = null;
        _elapsedBeforeCurrentRun = finalTime;
        _ = TimerCueService.PlayCompletionSequenceAsync();
        UpdateVisualState();
        _ = DisplayAlertAsync("Workout completed", "Timer finished. You can go back or start a new workout from the main menu.", "OK");
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

    private TimeSpan GetElapsed()
    {
        return _elapsedBeforeCurrentRun +
               (_currentRunStartedAtUtc is null
                   ? TimeSpan.Zero
                   : DateTimeOffset.UtcNow - _currentRunStartedAtUtc.Value);
    }

    private async void OnSlideResetCompleted(object? sender, EventArgs e)
    {
        await PlayConfettiAsync("WOD IS OVER");
        ResetWorkoutSession();
    }

    private void ResetWorkoutSession()
    {
        _timer.Stop();
        _state = WorkoutTimerState.Idle;
        _preCountdownStartedAtUtc = null;
        _currentRunStartedAtUtc = null;
        _elapsedBeforeCurrentRun = TimeSpan.Zero;
        _roundCount = 0;
        RoundButton.Text = "00";
        ProgressRing.Progress = 0;
        ResetCueTracking();
        UpdateVisualState();
        SlideToReset.Reset();
    }

    private void UpdateVisualState()
    {
        TimerActionButton.Source = PlayIcon;
        PauseActionButton.Source = PauseIcon;
        PauseActionButton.IsVisible = false;
        RoundButton.IsEnabled = _state == WorkoutTimerState.Running;
        RoundButton.Opacity = _state == WorkoutTimerState.Running ? 1 : 0.55;
        CenterActionShell.IsVisible = true;

        switch (_state)
        {
            case WorkoutTimerState.Idle:
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = string.Empty;
                PausedTimeLabel.IsVisible = false;
                ProgressRing.Progress = 0;
                TimerActionButton.IsEnabled = true;
                TimerActionButton.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                CenterActionShell.IsVisible = true;
                break;
            case WorkoutTimerState.PreCountdown:
                StateValueLabel.Text = "10";
                StateHintLabel.Text = "Tap to cancel";
                PausedTimeLabel.IsVisible = false;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                CenterActionShell.IsVisible = false;
                break;
            case WorkoutTimerState.Running:
                StateValueLabel.Text = _configuration.CountsDown
                    ? FormatClock(TimeSpan.FromSeconds(_configuration.DurationSeconds) - GetElapsed())
                    : FormatClock(GetElapsed());
                StateHintLabel.Text = "Tap the time or pause";
                PausedTimeLabel.IsVisible = false;
                PauseActionButton.IsVisible = true;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                CenterActionShell.IsVisible = false;
                break;
            case WorkoutTimerState.Paused:
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = string.Empty;
                PausedTimeLabel.IsVisible = false;
                TimerActionButton.IsEnabled = true;
                TimerActionButton.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                CenterActionShell.IsVisible = true;
                break;
            case WorkoutTimerState.Completed:
                StateValueLabel.Text = _configuration.CountsDown ? "00:00" : FormatClock(_elapsedBeforeCurrentRun);
                StateHintLabel.Text = "Workout completed. Go back to start a new one.";
                PausedTimeLabel.IsVisible = false;
                ProgressRing.Progress = 1;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                CenterActionShell.IsVisible = false;
                break;
        }
    }

    private async Task PlayConfettiAsync(string? overlayMessage = null)
    {
        ConfettiOverlay.Children.Clear();

        Label? messageLabel = null;
        if (!string.IsNullOrWhiteSpace(overlayMessage))
        {
            messageLabel = new Label
            {
                Text = overlayMessage,
                Style = (Style)Application.Current!.Resources["PageTitleStyle"],
                FontSize = 32,
                CharacterSpacing = 2,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Opacity = 0
            };

            ConfettiOverlay.Children.Add(messageLabel);
            await messageLabel.FadeToAsync(1, 220, Easing.CubicIn);
        }

        var colors = new[]
        {
            Color.FromArgb("#FF3B3B"),
            Color.FromArgb("#423BFF"),
            Color.FromArgb("#FFD166"),
            Color.FromArgb("#35D07F")
        };

        var animationTasks = new List<Task>();

        for (var index = 0; index < 24; index++)
        {
            var particle = new BoxView
            {
                WidthRequest = 10,
                HeightRequest = 10,
                Color = colors[index % colors.Length],
                CornerRadius = 2,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Opacity = 0.95
            };

            ConfettiOverlay.Children.Add(particle);

            var angle = (Math.PI * 2 * index) / 24;
            var distance = 180 + Random.Shared.Next(40, 220);
            var x = Math.Cos(angle) * distance;
            var y = Math.Sin(angle) * distance;

            animationTasks.Add(AnimateParticleAsync(particle, x, y));
        }

        await Task.WhenAll(animationTasks);

        if (messageLabel is not null)
        {
            await messageLabel.FadeToAsync(0, 260, Easing.CubicOut);
        }

        ConfettiOverlay.Children.Clear();
    }

    private static async Task AnimateParticleAsync(BoxView particle, double x, double y)
    {
        await Task.WhenAll(
            particle.TranslateToAsync(x, y, 1200, Easing.CubicOut),
            particle.RotateToAsync(Random.Shared.Next(-180, 180), 1200, Easing.CubicOut),
            particle.FadeToAsync(0, 1200, Easing.CubicIn));
    }

    private static string BuildWorkoutDetails(WorkoutConfigurationRequest configuration)
    {
        var details = new StringBuilder();

        if (!string.IsNullOrWhiteSpace(configuration.HeroWodName))
        {
            details.AppendLine(configuration.HeroWodName.Trim());
        }

        if (!string.IsNullOrWhiteSpace(configuration.WodDescription))
        {
            if (details.Length > 0)
            {
                details.AppendLine();
            }

            details.Append(configuration.WodDescription.Trim());
        }

        if (!string.IsNullOrWhiteSpace(configuration.Notes))
        {
            if (details.Length > 0)
            {
                details.AppendLine();
                details.AppendLine();
            }

            details.Append(configuration.Notes.Trim());
        }

        return details.ToString();
    }

    private static string BuildWatermarkMinutes(WorkoutConfigurationRequest configuration)
    {
        var totalSeconds = configuration.DurationSeconds > 0
            ? configuration.DurationSeconds
            : configuration.TimeCapSeconds.GetValueOrDefault();

        if (totalSeconds <= 0)
        {
            return "0";
        }

        return Math.Max(1, (int)Math.Ceiling(totalSeconds / 60d)).ToString();
    }

    private static string BuildContextLabel(WorkoutConfigurationRequest configuration)
    {
        var totalSeconds = configuration.DurationSeconds > 0
            ? configuration.DurationSeconds
            : configuration.TimeCapSeconds.GetValueOrDefault();

        if (totalSeconds > 0)
        {
            return configuration.CountsDown
                ? $"{FormatClock(TimeSpan.FromSeconds(totalSeconds))} MINUTES"
                : $"{FormatClock(TimeSpan.FromSeconds(totalSeconds))} CAP";
        }

        return configuration.TypeDisplayName.ToUpperInvariant();
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
