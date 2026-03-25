using Trimex.Models;

namespace Trimex.Pages;

public partial class TabataConfigurationPage : ContentPage
{
    private int _rounds = 8;
    private int _workSeconds = 20;
    private int _restSeconds = 10;

    public TabataConfigurationPage()
    {
        InitializeComponent();
        BuildRoundsOverlayItems();
        BuildTimeOverlayItems(WorkTimeItemsLayout, OnWorkTimeItemSelected);
        BuildTimeOverlayItems(RestTimeItemsLayout, OnRestTimeItemSelected);
        UpdateDisplays();
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
        UpdateDisplays();
        RoundsOverlay.IsVisible = false;
    }

    // --- Work time overlay ---

    private void OnWorkTimeTapped(object? sender, TappedEventArgs e) =>
        WorkTimeOverlay.IsVisible = true;

    private void OnWorkTimeOverlayBackgroundTapped(object? sender, TappedEventArgs e) =>
        WorkTimeOverlay.IsVisible = false;

    private void OnWorkTimeOverlayCancelClicked(object? sender, EventArgs e) =>
        WorkTimeOverlay.IsVisible = false;

    private void OnWorkTimeItemSelected(int seconds)
    {
        _workSeconds = seconds;
        UpdateDisplays();
        WorkTimeOverlay.IsVisible = false;
    }

    // --- Rest time overlay ---

    private void OnRestTimeTapped(object? sender, TappedEventArgs e) =>
        RestTimeOverlay.IsVisible = true;

    private void OnRestTimeOverlayBackgroundTapped(object? sender, TappedEventArgs e) =>
        RestTimeOverlay.IsVisible = false;

    private void OnRestTimeOverlayCancelClicked(object? sender, EventArgs e) =>
        RestTimeOverlay.IsVisible = false;

    private void OnRestTimeItemSelected(int seconds)
    {
        _restSeconds = seconds;
        UpdateDisplays();
        RestTimeOverlay.IsVisible = false;
    }

    // --- Navigation ---

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var request = new TabataConfigurationRequest
        {
            Rounds = _rounds,
            WorkSeconds = _workSeconds,
            RestSeconds = _restSeconds
        };

        await Navigation.PushAsync(new TabataTimerPage(request));
    }

    // --- Display helpers ---

    private void UpdateDisplays()
    {
        RoundsDisplayLabel.Text = $"{_rounds} ROUNDS";
        WorkTimeDisplayLabel.Text = FormatDurationLabel(_workSeconds);
        RestTimeDisplayLabel.Text = FormatDurationLabel(_restSeconds);
    }

    private void BuildRoundsOverlayItems()
    {
        for (var r = 1; r <= 100; r++)
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

    private void BuildTimeOverlayItems(VerticalStackLayout layout, Action<int> onSelected)
    {
        // 5s to 55s by 5s increments
        for (var s = 5; s < 60; s += 5)
            AddTimeItem(layout, s, onSelected);

        // 1:00 to 15:00 by 30s increments
        for (var s = 60; s <= 900; s += 30)
            AddTimeItem(layout, s, onSelected);
    }

    private static void AddTimeItem(VerticalStackLayout layout, int seconds, Action<int> onSelected)
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
        tap.Tapped += (_, _) => onSelected(seconds);
        label.GestureRecognizers.Add(tap);
        layout.Children.Add(label);
    }

    internal static string FormatDurationLabel(int seconds) =>
        seconds < 60
            ? $"{seconds} seconds"
            : $"{seconds / 60:00}:{seconds % 60:00}";
}
