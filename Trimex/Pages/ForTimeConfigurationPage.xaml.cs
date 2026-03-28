using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class ForTimeConfigurationPage : ContentPage
{
    private const int MinValue = 1;
    private const int MaxValue = 100;
    private const double PanThreshold = 30;

    // Inertia constants
    private const double Friction = 0.93;
    private const double MinInertiaVelocity = 50;
    private const int InertiaIntervalMs = 16;

    private int _selectedMinutes = 15;
    private double _lastPanY;
    private double _panAccumulator;

    // Velocity / inertia tracking
    private double _velocityY;
    private DateTime _lastPanTime;
    private IDispatcherTimer? _inertiaTimer;
    private double _inertiaAccumulator;

    public ForTimeConfigurationPage(IHeroWodRepository heroWodRepository, IWorkoutNoteRepository workoutNoteRepository)
    {
        InitializeComponent();

        UpdateCarouselDisplay();
    }

    private void OnPickerPanUpdated(object? sender, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                StopInertia();
                _lastPanY = 0;
                _panAccumulator = 0;
                _velocityY = 0;
                _lastPanTime = DateTime.UtcNow;
                break;

            case GestureStatus.Running:
                var delta = e.TotalY - _lastPanY;
                _lastPanY = e.TotalY;
                _panAccumulator += delta;

                var now = DateTime.UtcNow;
                var dt = (now - _lastPanTime).TotalSeconds;
                _lastPanTime = now;

                if (dt is > 0 and < 0.3)
                    _velocityY = _velocityY * 0.3 + (delta / dt) * 0.7;

                ApplySteps(ref _panAccumulator);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (Math.Abs(_velocityY) > MinInertiaVelocity)
                    StartInertia();
                else
                    _velocityY = 0;

                _panAccumulator = 0;
                _lastPanY = 0;
                break;
        }
    }

    private void ApplySteps(ref double accumulator)
    {
        while (accumulator <= -PanThreshold)
        {
            accumulator += PanThreshold;
            if (_selectedMinutes < MaxValue)
            {
                _selectedMinutes++;
                UpdateCarouselDisplay();
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            }
        }

        while (accumulator >= PanThreshold)
        {
            accumulator -= PanThreshold;
            if (_selectedMinutes > MinValue)
            {
                _selectedMinutes--;
                UpdateCarouselDisplay();
                HapticFeedback.Default.Perform(HapticFeedbackType.Click);
            }
        }
    }

    private void StartInertia()
    {
        StopInertia();
        _inertiaAccumulator = 0;

        _inertiaTimer = Dispatcher.CreateTimer();
        _inertiaTimer.Interval = TimeSpan.FromMilliseconds(InertiaIntervalMs);
        _inertiaTimer.Tick += OnInertiaTick;
        _inertiaTimer.Start();
    }

    private void StopInertia()
    {
        if (_inertiaTimer is null)
            return;

        _inertiaTimer.Stop();
        _inertiaTimer.Tick -= OnInertiaTick;
        _inertiaTimer = null;
        _velocityY = 0;
    }

    private void OnInertiaTick(object? sender, EventArgs e)
    {
        _inertiaAccumulator += _velocityY * (InertiaIntervalMs / 1000.0);
        _velocityY *= Friction;

        ApplySteps(ref _inertiaAccumulator);

        var hitBound = (_selectedMinutes <= MinValue && _velocityY > 0)
                    || (_selectedMinutes >= MaxValue && _velocityY < 0);

        if (Math.Abs(_velocityY) < MinInertiaVelocity || hitBound)
            StopInertia();
    }

    private void OnQuickSelectClicked(object? sender, EventArgs e)
    {
        StopInertia();
        if (sender is Button btn && btn.CommandParameter is string val && int.TryParse(val, out var minutes))
        {
            _selectedMinutes = Math.Clamp(minutes, MinValue, MaxValue);
            UpdateCarouselDisplay();
        }
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var selectedMinutes = _selectedMinutes;
        int? capSeconds = selectedMinutes * 60;

        var request = new WorkoutConfigurationRequest
        {
            Type = WorkoutTypes.ForTime,
            TypeDisplayName = "FOR TIME",
            DurationSeconds = capSeconds.Value,
            TimeCapSeconds = capSeconds,
            DurationLabel = $"{selectedMinutes} minutes",
            Notes = string.Empty,
            HeroWodUniqueId = null,
            HeroWodName = string.Empty,
            WodDescription = string.Empty,
            CountsDown = false,
            SupportsRounds = false
        };

        await Navigation.PushAsync(new WorkoutTimerPage(request));
    }

    private void UpdateCarouselDisplay()
    {
        TimeCapDisplayLabel.Text = $"{_selectedMinutes:00}:00";

        PickerSlotMinus2.Text = _selectedMinutes - 2 >= MinValue ? $"{_selectedMinutes - 2:00}:00" : "";
        PickerSlotMinus1.Text = _selectedMinutes - 1 >= MinValue ? $"{_selectedMinutes - 1:00}:00" : "";
        PickerSlotPlus1.Text = _selectedMinutes + 1 <= MaxValue ? $"{_selectedMinutes + 1:00}:00" : "";
        PickerSlotPlus2.Text = _selectedMinutes + 2 <= MaxValue ? $"{_selectedMinutes + 2:00}:00" : "";

        UpdatePillStates();
    }

    private void UpdatePillStates()
    {
        var inactive = Color.FromArgb("#262626");
        var active   = Color.FromArgb("#FF3B3B");

        (Button pill, int value)[] pills =
        [
            (Pill5,  5),
            (Pill10, 10),
            (Pill15, 15),
            (Pill20, 20)
        ];

        foreach (var (pill, value) in pills)
            pill.BackgroundColor = _selectedMinutes == value ? active : inactive;
    }
}
