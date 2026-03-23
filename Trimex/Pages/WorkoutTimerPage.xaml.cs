using Trimex.Models;

namespace Trimex.Pages;

public partial class WorkoutTimerPage : ContentPage
{
    private readonly WorkoutConfigurationRequest _configuration;
    private readonly IDispatcherTimer _timer;

    private WorkoutTimerState _state = WorkoutTimerState.Idle;
    private DateTimeOffset? _preCountdownStartedAtUtc;
    private DateTimeOffset? _currentRunStartedAtUtc;
    private TimeSpan _elapsedBeforeCurrentRun = TimeSpan.Zero;
    private int _roundCount;

    public WorkoutTimerPage(WorkoutConfigurationRequest configuration)
    {
        InitializeComponent();

        _configuration = configuration;
        Title = configuration.TypeDisplayName;

        WorkoutTitleLabel.Text = configuration.TypeDisplayName;
        WorkoutContextLabel.Text = BuildContextLabel(configuration);

        RoundSection.IsVisible = configuration.SupportsRounds;
        RoundCountLabel.Text = "0";

        ProgressRing.AccentColor = configuration.Type == WorkoutTypes.ForTime
            ? Color.FromArgb("#FF3B3B")
            : Color.FromArgb("#423BFF");

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

    private async void OnRoundClicked(object? sender, EventArgs e)
    {
        _roundCount++;
        RoundCountLabel.Text = _roundCount.ToString();
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
        _state = WorkoutTimerState.PreCountdown;
        _preCountdownStartedAtUtc = DateTimeOffset.UtcNow;
        _timer.Start();
        UpdateVisualState();
    }

    private void PauseWorkout()
    {
        if (_currentRunStartedAtUtc is not null)
        {
            _elapsedBeforeCurrentRun += DateTimeOffset.UtcNow - _currentRunStartedAtUtc.Value;
            _currentRunStartedAtUtc = null;
        }

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
            UpdateVisualState();
            return;
        }

        StateValueLabel.Text = remaining.ToString();
        StateHintLabel.Text = "Get ready";
        PausedTimeLabel.IsVisible = false;
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

            StateValueLabel.Text = FormatClock(remaining);
            StateHintLabel.Text = "Tap the timer to pause";
            ProgressRing.Progress = elapsed.TotalSeconds / _configuration.DurationSeconds;
            PausedTimeLabel.IsVisible = false;

            return;
        }

        StateValueLabel.Text = FormatClock(elapsed);
        StateHintLabel.Text = "Tap the timer to pause";
        PausedTimeLabel.IsVisible = false;

        if (_configuration.TimeCapSeconds is int timeCapSeconds && timeCapSeconds > 0)
        {
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
        UpdateVisualState();
        _ = DisplayAlertAsync("Workout completed", "Timer finished. You can go back or start a new workout from the main menu.", "OK");
    }

    private TimeSpan GetElapsed()
    {
        return _elapsedBeforeCurrentRun +
               (_currentRunStartedAtUtc is null
                   ? TimeSpan.Zero
                   : DateTimeOffset.UtcNow - _currentRunStartedAtUtc.Value);
    }

    private void UpdateVisualState()
    {
        switch (_state)
        {
            case WorkoutTimerState.Idle:
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = string.Empty;
                PausedTimeLabel.IsVisible = false;
                ProgressRing.Progress = 0;
                TimerActionButton.Text = "PLAY";
                TimerActionButton.IsEnabled = true;
                TimerActionButton.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                break;
            case WorkoutTimerState.PreCountdown:
                StateValueLabel.Text = "10";
                StateHintLabel.Text = "Get ready";
                PausedTimeLabel.IsVisible = false;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;
            case WorkoutTimerState.Running:
                StateValueLabel.Text = _configuration.CountsDown
                    ? FormatClock(TimeSpan.FromSeconds(_configuration.DurationSeconds) - GetElapsed())
                    : FormatClock(GetElapsed());
                StateHintLabel.Text = "Tap the time to pause";
                PausedTimeLabel.IsVisible = false;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;
            case WorkoutTimerState.Paused:
                StateValueLabel.Text = string.Empty;
                StateHintLabel.Text = string.Empty;
                PausedTimeLabel.IsVisible = false;
                TimerActionButton.Text = "RESUME";
                TimerActionButton.IsEnabled = true;
                TimerActionButton.IsVisible = true;
                TimerDisplayLayout.IsVisible = false;
                break;
            case WorkoutTimerState.Completed:
                StateValueLabel.Text = _configuration.CountsDown ? "00:00" : FormatClock(_elapsedBeforeCurrentRun);
                StateHintLabel.Text = "Workout completed. Go back to start a new one.";
                PausedTimeLabel.IsVisible = false;
                ProgressRing.Progress = 1;
                TimerActionButton.IsVisible = false;
                TimerDisplayLayout.IsVisible = true;
                break;
        }
    }

    private async Task PlayConfettiAsync()
    {
        ConfettiOverlay.Children.Clear();

        var colors = new[]
        {
            Color.FromArgb("#FF3B3B"),
            Color.FromArgb("#423BFF"),
            Color.FromArgb("#FFD166"),
            Color.FromArgb("#35D07F")
        };

        var animationTasks = new List<Task>();

        for (var index = 0; index < 16; index++)
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

            var angle = (Math.PI * 2 * index) / 16;
            var distance = 90 + Random.Shared.Next(20, 80);
            var x = Math.Cos(angle) * distance;
            var y = Math.Sin(angle) * distance;

            animationTasks.Add(AnimateParticleAsync(particle, x, y));
        }

        await Task.WhenAll(animationTasks);
        ConfettiOverlay.Children.Clear();
    }

    private static async Task AnimateParticleAsync(BoxView particle, double x, double y)
    {
        await Task.WhenAll(
            particle.TranslateToAsync(x, y, 1200, Easing.CubicOut),
            particle.RotateToAsync(Random.Shared.Next(-180, 180), 1200, Easing.CubicOut),
            particle.FadeToAsync(0, 1200, Easing.CubicIn));
    }

    private static string BuildContextLabel(WorkoutConfigurationRequest configuration)
    {
        var parts = new List<string> { configuration.DurationLabel };

        if (!string.IsNullOrWhiteSpace(configuration.HeroWodName))
        {
            parts.Add(configuration.HeroWodName);
        }

        if (!string.IsNullOrWhiteSpace(configuration.Notes))
        {
            parts.Add(configuration.Notes);
        }

        return string.Join("  |  ", parts);
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
