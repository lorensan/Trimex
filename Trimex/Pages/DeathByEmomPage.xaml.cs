namespace Trimex.Pages;

public partial class DeathByEmomPage : ContentPage
{
    private int _selectedIntervalSeconds = 60;

    public DeathByEmomPage()
    {
        InitializeComponent();
        BuildDurationOverlayItems();
        UpdateDurationDisplay();
    }

    private void OnDurationTapped(object? sender, TappedEventArgs e)
    {
        DurationOverlay.IsVisible = true;
    }

    private void OnOverlayBackgroundTapped(object? sender, TappedEventArgs e)
    {
        DurationOverlay.IsVisible = false;
    }

    private void OnOverlayCancelClicked(object? sender, EventArgs e)
    {
        DurationOverlay.IsVisible = false;
    }

    private void OnDurationItemSelected(int seconds)
    {
        _selectedIntervalSeconds = seconds;
        UpdateDurationDisplay();
        DurationOverlay.IsVisible = false;
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new DeathByEmomTimerPage(_selectedIntervalSeconds));
    }

    private void UpdateDurationDisplay()
    {
        DurationDisplayLabel.Text = EmomConfigurationPage.FormatDurationLabel(_selectedIntervalSeconds);
    }

    private void BuildDurationOverlayItems()
    {
        // 15s to 10:00 by 15s increments
        for (var s = 15; s <= 600; s += 15)
        {
            var seconds = s;
            var label = new Label
            {
                Text = EmomConfigurationPage.FormatDurationLabel(seconds),
                FontSize = 17,
                FontFamily = "OpenSansSemibold",
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(0, 10)
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => OnDurationItemSelected(seconds);
            label.GestureRecognizers.Add(tap);
            DurationItemsLayout.Children.Add(label);
        }
    }
}
