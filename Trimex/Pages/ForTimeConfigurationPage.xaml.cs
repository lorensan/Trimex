using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class ForTimeConfigurationPage : ContentPage
{
    private int _selectedMinutes = 15;

    public ForTimeConfigurationPage(IHeroWodRepository heroWodRepository, IWorkoutNoteRepository workoutNoteRepository)
    {
        InitializeComponent();

        BuildTimeCapOverlayItems();
        UpdateTimeCapDisplay();
    }

    private void OnTimeCapTapped(object? sender, TappedEventArgs e) =>
        TimeCapOverlay.IsVisible = true;

    private void OnOverlayBackgroundTapped(object? sender, TappedEventArgs e) =>
        TimeCapOverlay.IsVisible = false;

    private void OnOverlayCancelClicked(object? sender, EventArgs e) =>
        TimeCapOverlay.IsVisible = false;

    private void OnTimeCapItemSelected(int minutes)
    {
        _selectedMinutes = minutes;
        UpdateTimeCapDisplay();
        TimeCapOverlay.IsVisible = false;
    }

    private void OnQuickSelectClicked(object? sender, EventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is string val && int.TryParse(val, out var minutes))
        {
            _selectedMinutes = minutes;
            UpdateTimeCapDisplay();
        }
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var selectedMinutes = _selectedMinutes;
        int? capSeconds = selectedMinutes == 0 ? null : selectedMinutes * 60;

        var request = new WorkoutConfigurationRequest
        {
            Type = WorkoutTypes.ForTime,
            TypeDisplayName = "FOR TIME",
            DurationSeconds = capSeconds ?? 0,
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

    private void UpdateTimeCapDisplay()
    {
        TimeCapDisplayLabel.Text = _selectedMinutes == 0
            ? "∞"
            : $"{_selectedMinutes:00}:00";
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

    private void BuildTimeCapOverlayItems()
    {
        for (var minute = 0; minute <= 100; minute++)
        {
            var m = minute;
            var label = new Label
            {
                Text = minute == 0 ? "No cap (manual stop)" : $"{minute} minutes",
                FontSize = 17,
                FontFamily = "OpenSansSemibold",
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(0, 10)
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (_, _) => OnTimeCapItemSelected(m);
            label.GestureRecognizers.Add(tapGesture);

            TimeCapItemsLayout.Children.Add(label);
        }
    }
}
