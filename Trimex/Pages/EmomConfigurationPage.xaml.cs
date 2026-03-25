using Trimex.Models;

namespace Trimex.Pages;

public partial class EmomConfigurationPage : ContentPage
{
    private int _selectedIntervalSeconds = 30;
    private int _rounds = 10;

    public EmomConfigurationPage()
    {
        InitializeComponent();
        BuildDurationOverlayItems();
        BuildRoundsOverlayItems();
        UpdateDurationDisplay();
        UpdateRoundsDisplay();
        UpdateRoundsSummary();
    }

    // --- Duration overlay ---

    private void OnDurationTapped(object? sender, TappedEventArgs e) =>
        DurationOverlay.IsVisible = true;

    private void OnDurationOverlayBackgroundTapped(object? sender, TappedEventArgs e) =>
        DurationOverlay.IsVisible = false;

    private void OnDurationOverlayCancelClicked(object? sender, EventArgs e) =>
        DurationOverlay.IsVisible = false;

    private void OnDurationItemSelected(int seconds)
    {
        _selectedIntervalSeconds = seconds;
        UpdateDurationDisplay();
        UpdateRoundsSummary();
        DurationOverlay.IsVisible = false;
    }

    // --- Rounds overlay ---

    private void OnRoundsTapped(object? sender, TappedEventArgs e) =>
        RoundsOverlay.IsVisible = true;

    private void OnRoundsOverlayBackgroundTapped(object? sender, TappedEventArgs e) =>
        RoundsOverlay.IsVisible = false;

    private void OnRoundsOverlayCancelClicked(object? sender, EventArgs e) =>
        RoundsOverlay.IsVisible = false;

    private void OnRoundsItemSelected(int rounds)
    {
        _rounds = rounds;
        UpdateRoundsDisplay();
        UpdateRoundsSummary();
        RoundsOverlay.IsVisible = false;
    }

    // --- Navigation ---

    private async void OnDeathByClicked(object? sender, EventArgs e) =>
        await Navigation.PushAsync(new DeathByEmomPage());

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var request = new EmomConfigurationRequest
        {
            IntervalSeconds = _selectedIntervalSeconds,
            Rounds = _rounds
        };

        await Navigation.PushAsync(new EmomTimerPage(request));
    }

    // --- Display helpers ---

    private void UpdateDurationDisplay() =>
        DurationDisplayLabel.Text = FormatDurationLabel(_selectedIntervalSeconds);

    private void UpdateRoundsDisplay() =>
        RoundsDisplayLabel.Text = _rounds.ToString();

    private void UpdateRoundsSummary()
    {
        var total = TimeSpan.FromSeconds((double)_selectedIntervalSeconds * _rounds);
        var totalMinutes = (int)total.TotalMinutes;
        RoundsSummaryLabel.Text = $"{_rounds}  /  {totalMinutes:00}:{total.Seconds:00}";
    }

    private void BuildDurationOverlayItems()
    {
        // 15s to 55s by 5s increments
        for (var s = 15; s < 60; s += 5)
        {
            AddDurationItem(s);
        }

        // 1:00 to 10:00 by 15s increments
        for (var s = 60; s <= 600; s += 15)
        {
            AddDurationItem(s);
        }
    }

    private void AddDurationItem(int seconds)
    {
        var label = new Label
        {
            Text = FormatDurationLabel(seconds),
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

    private void BuildRoundsOverlayItems()
    {
        for (var r = 1; r <= 10; r++)
        {
            var rounds = r;
            var label = new Label
            {
                Text = rounds.ToString(),
                FontSize = 17,
                FontFamily = "OpenSansSemibold",
                TextColor = Color.FromArgb("#FFFFFF"),
                HorizontalTextAlignment = TextAlignment.Center,
                Padding = new Thickness(0, 10)
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => OnRoundsItemSelected(rounds);
            label.GestureRecognizers.Add(tap);
            RoundsItemsLayout.Children.Add(label);
        }
    }

    internal static string FormatDurationLabel(int seconds) =>
        seconds < 60
            ? $"{seconds} seconds"
            : $"{seconds / 60:00}:{seconds % 60:00}";
}
