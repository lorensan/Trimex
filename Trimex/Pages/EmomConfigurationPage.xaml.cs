using Trimex.Models;

namespace Trimex.Pages;

public partial class EmomConfigurationPage : ContentPage
{
    private const double PanThreshold = 30;
    private const double Friction = 0.93;
    private const double MinInertiaVelocity = 50;
    private const int InertiaIntervalMs = 16;

    private static readonly int[] DurationSteps = BuildDurationSteps();

    private readonly PickerState _durationPicker;
    private readonly PickerState _roundsPicker;

    public EmomConfigurationPage()
    {
        InitializeComponent();

        _durationPicker = new PickerState(0, DurationSteps.Length - 1, Array.IndexOf(DurationSteps, 30));
        _roundsPicker = new PickerState(0, 99, 99);

        UpdateAllDisplays();
    }

    // --- Pan handlers ---

    private void OnDurationPanUpdated(object? sender, PanUpdatedEventArgs e) =>
        HandlePan(_durationPicker, e);

    private void OnRoundsPanUpdated(object? sender, PanUpdatedEventArgs e) =>
        HandlePan(_roundsPicker, e);

    // --- Shared pan + inertia ---

    private void HandlePan(PickerState state, PanUpdatedEventArgs e)
    {
        switch (e.StatusType)
        {
            case GestureStatus.Started:
                StopInertia(state);
                state.LastPanY = 0;
                state.PanAccumulator = 0;
                state.VelocityY = 0;
                state.LastPanTime = DateTime.UtcNow;
                break;

            case GestureStatus.Running:
                var delta = e.TotalY - state.LastPanY;
                state.LastPanY = e.TotalY;
                state.PanAccumulator += delta;

                var now = DateTime.UtcNow;
                var dt = (now - state.LastPanTime).TotalSeconds;
                state.LastPanTime = now;

                if (dt is > 0 and < 0.3)
                    state.VelocityY = state.VelocityY * 0.3 + (delta / dt) * 0.7;

                ApplySteps(ref state.PanAccumulator, state);
                break;

            case GestureStatus.Completed:
            case GestureStatus.Canceled:
                if (Math.Abs(state.VelocityY) > MinInertiaVelocity)
                    StartInertia(state);
                else
                    state.VelocityY = 0;

                state.PanAccumulator = 0;
                state.LastPanY = 0;
                break;
        }
    }

    private void ApplySteps(ref double accumulator, PickerState state)
    {
        var changed = false;

        while (accumulator <= -PanThreshold)
        {
            accumulator += PanThreshold;
            if (state.Index < state.MaxIndex) { state.Index++; changed = true; }
        }

        while (accumulator >= PanThreshold)
        {
            accumulator -= PanThreshold;
            if (state.Index > state.MinIndex) { state.Index--; changed = true; }
        }

        if (changed)
        {
            UpdateAllDisplays();
            HapticFeedback.Default.Perform(HapticFeedbackType.Click);
        }
    }

    private void StartInertia(PickerState state)
    {
        StopInertia(state);
        state.InertiaAccumulator = 0;

        state.InertiaTimer = Dispatcher.CreateTimer();
        state.InertiaTimer.Interval = TimeSpan.FromMilliseconds(InertiaIntervalMs);
        state.InertiaTimer.Tick += (_, _) =>
        {
            state.InertiaAccumulator += state.VelocityY * (InertiaIntervalMs / 1000.0);
            state.VelocityY *= Friction;

            ApplySteps(ref state.InertiaAccumulator, state);

            var hitBound = (state.Index <= state.MinIndex && state.VelocityY > 0)
                        || (state.Index >= state.MaxIndex && state.VelocityY < 0);

            if (Math.Abs(state.VelocityY) < MinInertiaVelocity || hitBound)
                StopInertia(state);
        };
        state.InertiaTimer.Start();
    }

    private static void StopInertia(PickerState state)
    {
        if (state.InertiaTimer is null) return;
        state.InertiaTimer.Stop();
        state.InertiaTimer = null;
        state.VelocityY = 0;
    }

    // --- Navigation ---

    private async void OnDeathByTapped(object? sender, TappedEventArgs e) =>
        await Navigation.PushAsync(new DeathByEmomPage());

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var request = new EmomConfigurationRequest
        {
            IntervalSeconds = DurationSteps[_durationPicker.Index],
            Rounds = _roundsPicker.Index + 1
        };

        await Navigation.PushAsync(new EmomTimerPage(request));
    }

    // --- Display helpers ---

    private void UpdateAllDisplays()
    {
        var durationSeconds = DurationSteps[_durationPicker.Index];
        var rounds = _roundsPicker.Index + 1;

        DurationDisplayLabel.Text = FormatMmSs(durationSeconds);
        DurationSlotMinus2.Text = _durationPicker.Index - 2 >= 0 ? FormatMmSs(DurationSteps[_durationPicker.Index - 2]) : "";
        DurationSlotMinus1.Text = _durationPicker.Index - 1 >= 0 ? FormatMmSs(DurationSteps[_durationPicker.Index - 1]) : "";
        DurationSlotPlus1.Text = _durationPicker.Index + 1 < DurationSteps.Length ? FormatMmSs(DurationSteps[_durationPicker.Index + 1]) : "";
        DurationSlotPlus2.Text = _durationPicker.Index + 2 < DurationSteps.Length ? FormatMmSs(DurationSteps[_durationPicker.Index + 2]) : "";

        RoundsDisplayLabel.Text = rounds.ToString();
        RoundsSlotMinus2.Text = rounds - 2 >= 1 ? (rounds - 2).ToString() : "";
        RoundsSlotMinus1.Text = rounds - 1 >= 1 ? (rounds - 1).ToString() : "";
        RoundsSlotPlus1.Text = rounds + 1 <= _roundsPicker.MaxIndex + 1 ? (rounds + 1).ToString() : "";
        RoundsSlotPlus2.Text = rounds + 2 <= _roundsPicker.MaxIndex + 1 ? (rounds + 2).ToString() : "";

        RoundsSummaryLabel.Text = rounds.ToString();
        var total = TimeSpan.FromSeconds((double)durationSeconds * rounds);
        TotalTimeSummaryLabel.Text = $"{(int)total.TotalMinutes:00}:{total.Seconds:00}";
    }

    private static string FormatMmSs(int totalSeconds) =>
        $"{totalSeconds / 60:00}:{totalSeconds % 60:00}";

    internal static string FormatDurationLabel(int seconds) =>
        seconds < 60
            ? $"{seconds} seconds"
            : $"{seconds / 60:00}:{seconds % 60:00}";

    private static int[] BuildDurationSteps()
    {
        var values = new List<int>();
        for (var s = 15; s < 60; s += 5) values.Add(s);
        for (var s = 60; s <= 600; s += 15) values.Add(s);
        return values.ToArray();
    }

    private sealed class PickerState
    {
        public int Index;
        public readonly int MinIndex;
        public readonly int MaxIndex;
        public double LastPanY;
        public double PanAccumulator;
        public double VelocityY;
        public DateTime LastPanTime;
        public IDispatcherTimer? InertiaTimer;
        public double InertiaAccumulator;

        public PickerState(int minIndex, int maxIndex, int initialIndex)
        {
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Index = initialIndex;
        }
    }
}
