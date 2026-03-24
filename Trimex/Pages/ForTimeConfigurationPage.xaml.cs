using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class ForTimeConfigurationPage : ContentPage
{
    private readonly IHeroWodRepository _heroWodRepository;
    private readonly IWorkoutNoteRepository _workoutNoteRepository;
    private readonly List<HeroWod> _heroWods = [];

    private HeroWod? _selectedHeroWod;
    private int _selectedMinutes = 15;

    public ForTimeConfigurationPage(IHeroWodRepository heroWodRepository, IWorkoutNoteRepository workoutNoteRepository)
    {
        InitializeComponent();
        _heroWodRepository = heroWodRepository;
        _workoutNoteRepository = workoutNoteRepository;

        BuildTimeCapOverlayItems();
        UpdateTimeCapDisplay();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _ = LoadHeroWodsAsync();
    }

    private async Task LoadHeroWodsAsync()
    {
        _heroWods.Clear();
        _heroWods.AddRange(await _heroWodRepository.GetByTypeAsync(WorkoutTypes.ForTime));

        HeroWodPicker.Items.Clear();
        HeroWodPicker.Items.Add("Custom FOR TIME");

        foreach (var heroWod in _heroWods)
        {
            HeroWodPicker.Items.Add(heroWod.Name);
        }

        HeroWodPicker.SelectedIndex = 0;
        HeroWodEmptyLabel.IsVisible = _heroWods.Count == 0;

        var savedNote = await _workoutNoteRepository.GetLatestAsync(WorkoutTypes.ForTime, null);
    }

    private async void OnHeroWodSelectionChanged(object? sender, EventArgs e)
    {
        _selectedHeroWod = HeroWodPicker.SelectedIndex <= 0
            ? null
            : _heroWods[HeroWodPicker.SelectedIndex - 1];

        HeroDescriptionBorder.IsVisible = _selectedHeroWod is not null;
        HeroDescriptionLabel.Text = _selectedHeroWod?.WodDescription ?? string.Empty;

        if (_selectedHeroWod?.TimeCap is int timeCap && timeCap >= 0)
        {
            _selectedMinutes = Math.Clamp(timeCap / 60, 0, 100);
            UpdateTimeCapDisplay();
        }
    }

    private void OnTimeCapTapped(object? sender, TappedEventArgs e)
    {
        TimeCapOverlay.IsVisible = true;
    }

    private void OnOverlayBackgroundTapped(object? sender, TappedEventArgs e)
    {
        TimeCapOverlay.IsVisible = false;
    }

    private void OnOverlayCancelClicked(object? sender, EventArgs e)
    {
        TimeCapOverlay.IsVisible = false;
    }

    private void OnTimeCapItemSelected(int minutes)
    {
        _selectedMinutes = minutes;
        UpdateTimeCapDisplay();
        TimeCapOverlay.IsVisible = false;
    }

    private async void OnAddNotesClicked(object? sender, EventArgs e)
    {
        var editedNote = await DisplayPromptAsync(
            "Workout notes",
            "Add the note you want to keep for this FOR TIME setup.",
            accept: "Save",
            cancel: "Cancel",
            placeholder: "Reps, split strategy, scaling...",
            maxLength: 500);

        if (editedNote is null)
        {
            return;
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
            HeroWodUniqueId = _selectedHeroWod?.UniqueId,
            HeroWodName = _selectedHeroWod?.Name ?? string.Empty,
            WodDescription = _selectedHeroWod?.WodDescription ?? string.Empty,
            CountsDown = false,
            SupportsRounds = false
        };

        await Navigation.PushAsync(new WorkoutTimerPage(request));
    }

    private void UpdateTimeCapDisplay()
    {
        TimeCapDisplayLabel.Text = _selectedMinutes == 0
            ? "No cap (manual stop)"
            : $"{_selectedMinutes} minutes";
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
