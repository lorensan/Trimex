namespace Trimex.Pages;

public partial class DeathByEmomPage : ContentPage
{
    private const double PanThreshold = 30;
    private const double Friction = 0.93;
    private const double MinInertiaVelocity = 50;
    private const int InertiaIntervalMs = 16;

    private static readonly int[] TimeSteps = BuildTimeSteps();

    private int _pickerIndex;
    private double _lastPanY;
    private double _panAccumulator;
    private double _velocityY;
    private DateTime _lastPanTime;
    private IDispatcherTimer? _inertiaTimer;
    private double _inertiaAccumulator;

    public DeathByEmomPage()
    {
        InitializeComponent();

        _pickerIndex = Array.IndexOf(TimeSteps, 60);
        UpdateCarouselDisplay();
    }

    // --- Pan + inertia ---

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
            if (_pickerIndex < TimeSteps.Length - 1)
            {
                _pickerIndex++;
                UpdateCarouselDisplay();
            }
        }

        while (accumulator >= PanThreshold)
        {
            accumulator -= PanThreshold;
            if (_pickerIndex > 0)
            {
                _pickerIndex--;
                UpdateCarouselDisplay();
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

        var hitBound = (_pickerIndex <= 0 && _velocityY > 0)
                    || (_pickerIndex >= TimeSteps.Length - 1 && _velocityY < 0);

        if (Math.Abs(_velocityY) < MinInertiaVelocity || hitBound)
            StopInertia();
    }

    // --- Navigation ---

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeathByEmomTimerPage(TimeSteps[_pickerIndex]));
    }

    // --- Display helpers ---

    private void UpdateCarouselDisplay()
    {
        DurationDisplayLabel.Text = FormatMmSs(TimeSteps[_pickerIndex]);

        PickerSlotMinus2.Text = _pickerIndex - 2 >= 0 ? FormatMmSs(TimeSteps[_pickerIndex - 2]) : "";
        PickerSlotMinus1.Text = _pickerIndex - 1 >= 0 ? FormatMmSs(TimeSteps[_pickerIndex - 1]) : "";
        PickerSlotPlus1.Text = _pickerIndex + 1 < TimeSteps.Length ? FormatMmSs(TimeSteps[_pickerIndex + 1]) : "";
        PickerSlotPlus2.Text = _pickerIndex + 2 < TimeSteps.Length ? FormatMmSs(TimeSteps[_pickerIndex + 2]) : "";
    }

    private static string FormatMmSs(int totalSeconds)
    {
        var minutes = totalSeconds / 60;
        var seconds = totalSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }

    private static int[] BuildTimeSteps()
    {
        var values = new List<int>();
        for (var s = 15; s <= 600; s += 15)
            values.Add(s);
        return values.ToArray();
    }
}
