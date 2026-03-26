using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class AmrapConfigurationPage : ContentPage
{
    private int _selectedMinutes = 20;

    public AmrapConfigurationPage(IHeroWodRepository heroWodRepository, IWorkoutNoteRepository workoutNoteRepository)
    {
        InitializeComponent();

        BuildMinutesOverlayItems();
        UpdateMinutesDisplay();
    }

    // --- Minutes card / overlay ---

    private void OnMinutesCardTapped(object? sender, TappedEventArgs e) =>
        MinutesOverlay.IsVisible = true;

    private void OnMinutesOverlayBackgroundTapped(object? sender, TappedEventArgs e) =>
        MinutesOverlay.IsVisible = false;

    private void OnMinutesOverlayCancelClicked(object? sender, EventArgs e) =>
        MinutesOverlay.IsVisible = false;

    private void OnMinutesItemSelected(int minutes)
    {
        _selectedMinutes = minutes;
        UpdateMinutesDisplay();
        MinutesOverlay.IsVisible = false;
    }

    // --- Quick select presets ---

    private void OnQuickSelectClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string val && int.TryParse(val, out var minutes))
        {
            _selectedMinutes = minutes;
            UpdateMinutesDisplay();
        }
    }

    // --- Navigation ---

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var request = new WorkoutConfigurationRequest
        {
            Type = WorkoutTypes.Amrap,
            TypeDisplayName = "AMRAP",
            DurationSeconds = _selectedMinutes * 60,
            TimeCapSeconds = _selectedMinutes * 60,
            DurationLabel = $"{_selectedMinutes} minutes",
            Notes = string.Empty,
            HeroWodUniqueId = null,
            HeroWodName = string.Empty,
            WodDescription = string.Empty,
            CountsDown = true,
            SupportsRounds = true
        };

        await Navigation.PushAsync(new WorkoutTimerPage(request));
    }

    // --- Display helpers ---

    private void UpdateMinutesDisplay()
    {
        MinutesNumberLabel.Text = $"{_selectedMinutes:00}";
        UpdatePresetStates();
        UpdateIntensityLabel();
    }

    private void UpdatePresetStates()
    {
        var active   = Color.FromArgb("#423BFF");
        var inactive = Color.FromArgb("#262626");

        (Button btn, int value)[] presets =
        [
            (Preset5,  5),
            (Preset10, 10),
            (Preset15, 15),
            (Preset20, 20)
        ];

        foreach (var (btn, value) in presets)
            btn.BackgroundColor = _selectedMinutes == value ? active : inactive;
    }

    private void UpdateIntensityLabel()
    {
        (IntensityLabel.Text, IntensityLabel.TextColor) = _selectedMinutes switch
        {
            <= 8  => ("SPRINT",           Color.FromArgb("#FF3B3B")),
            <= 15 => ("HIGH PERFORMANCE", Color.FromArgb("#35D07F")),
            _     => ("ENDURANCE",        Color.FromArgb("#423BFF"))
        };
    }

    private void BuildMinutesOverlayItems()
    {
        for (var minute = 1; minute <= 100; minute++)
        {
            var m = minute;
            var label = new Label
            {
                Text = $"{minute} min",
                FontSize = 17,
                FontFamily = "OpenSansSemibold",
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(0, 10)
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => OnMinutesItemSelected(m);
            label.GestureRecognizers.Add(tap);
            MinutesItemsLayout.Children.Add(label);
        }
    }
}
