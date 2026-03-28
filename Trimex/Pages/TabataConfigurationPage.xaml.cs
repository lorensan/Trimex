using Trimex.Models;

namespace Trimex.Pages;

public partial class TabataConfigurationPage : ContentPage
{
    private const double PanThreshold = 30;
    private const double Friction = 0.93;
    private const double MinInertiaVelocity = 50;
    private const int InertiaIntervalMs = 16;

    private static readonly int[] TimeSteps = BuildTimeSteps();

    private readonly PickerState _roundsPicker;
    private readonly PickerState _workPicker;
    private readonly PickerState _restPicker;

    public TabataConfigurationPage()
    {
        InitializeComponent();

        _roundsPicker = new PickerState(0, 99, 7);
        _workPicker = new PickerState(0, TimeSteps.Length - 1, Array.IndexOf(TimeSteps, 20));
        _restPicker = new PickerState(0, TimeSteps.Length - 1, Array.IndexOf(TimeSteps, 10));

        UpdateAllDisplays();
    }

    // --- Pan handlers ---

    private void OnRoundsPanUpdated(object? sender, PanUpdatedEventArgs e) =>
        HandlePan(_roundsPicker, e);

    private void OnWorkPanUpdated(object? sender, PanUpdatedEventArgs e) =>
        HandlePan(_workPicker, e);

    private void OnRestPanUpdated(object? sender, PanUpdatedEventArgs e) =>
        HandlePan(_restPicker, e);

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

                ApplySteps(state);
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

    private void ApplySteps(PickerState state)
    {
        var changed = false;

        while (state.PanAccumulator <= -PanThreshold)
        {
            state.PanAccumulator += PanThreshold;
            if (state.Index < state.MaxIndex) { state.Index++; changed = true; }
        }

        while (state.PanAccumulator >= PanThreshold)
        {
            state.PanAccumulator -= PanThreshold;
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

            ApplySteps(state);

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

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var request = new TabataConfigurationRequest
        {
            Rounds = _roundsPicker.Index + 1,
            WorkSeconds = TimeSteps[_workPicker.Index],
            RestSeconds = TimeSteps[_restPicker.Index]
        };

        await Navigation.PushAsync(new TabataTimerPage(request));
    }

    // --- Display helpers ---

    private void UpdateAllDisplays()
    {
        var rounds = _roundsPicker.Index + 1;
        var workSeconds = TimeSteps[_workPicker.Index];
        var restSeconds = TimeSteps[_restPicker.Index];

        RoundsNumberLabel.Text = rounds.ToString();
        RoundsSlotMinus2.Text = rounds - 2 >= 1 ? (rounds - 2).ToString() : "";
        RoundsSlotMinus1.Text = rounds - 1 >= 1 ? (rounds - 1).ToString() : "";
        RoundsSlotPlus1.Text = rounds + 1 <= 100 ? (rounds + 1).ToString() : "";
        RoundsSlotPlus2.Text = rounds + 2 <= 100 ? (rounds + 2).ToString() : "";

        WorkTimeDisplayLabel.Text = FormatMmSs(workSeconds);
        WorkSlotMinus2.Text = _workPicker.Index - 2 >= 0 ? FormatMmSs(TimeSteps[_workPicker.Index - 2]) : "";
        WorkSlotMinus1.Text = _workPicker.Index - 1 >= 0 ? FormatMmSs(TimeSteps[_workPicker.Index - 1]) : "";
        WorkSlotPlus1.Text = _workPicker.Index + 1 < TimeSteps.Length ? FormatMmSs(TimeSteps[_workPicker.Index + 1]) : "";
        WorkSlotPlus2.Text = _workPicker.Index + 2 < TimeSteps.Length ? FormatMmSs(TimeSteps[_workPicker.Index + 2]) : "";

        RestTimeDisplayLabel.Text = FormatMmSs(restSeconds);
        RestSlotMinus2.Text = _restPicker.Index - 2 >= 0 ? FormatMmSs(TimeSteps[_restPicker.Index - 2]) : "";
        RestSlotMinus1.Text = _restPicker.Index - 1 >= 0 ? FormatMmSs(TimeSteps[_restPicker.Index - 1]) : "";
        RestSlotPlus1.Text = _restPicker.Index + 1 < TimeSteps.Length ? FormatMmSs(TimeSteps[_restPicker.Index + 1]) : "";
        RestSlotPlus2.Text = _restPicker.Index + 2 < TimeSteps.Length ? FormatMmSs(TimeSteps[_restPicker.Index + 2]) : "";

        TotalDurationLabel.Text = FormatMmSs(rounds * (workSeconds + restSeconds));
    }

    private static string FormatMmSs(int totalSeconds)
    {
        var minutes = totalSeconds / 60;
        var seconds = totalSeconds % 60;
        return $"{minutes:00}:{seconds:00}";
    }

    internal static string FormatDurationLabel(int seconds) =>
        seconds < 60
            ? $"{seconds} seconds"
            : $"{seconds / 60:00}:{seconds % 60:00}";

    private static int[] BuildTimeSteps()
    {
        var values = new List<int>();
        for (var s = 5; s < 60; s += 5) values.Add(s);
        for (var s = 60; s <= 900; s += 30) values.Add(s);
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
