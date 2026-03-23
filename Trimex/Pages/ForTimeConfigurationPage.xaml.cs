using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class ForTimeConfigurationPage : ContentPage
{
    private readonly IHeroWodRepository _heroWodRepository;
    private readonly IWorkoutNoteRepository _workoutNoteRepository;
    private readonly List<HeroWod> _heroWods = [];

    private HeroWod? _selectedHeroWod;
    private string _notes = string.Empty;

    public ForTimeConfigurationPage(IHeroWodRepository heroWodRepository, IWorkoutNoteRepository workoutNoteRepository)
    {
        InitializeComponent();
        _heroWodRepository = heroWodRepository;
        _workoutNoteRepository = workoutNoteRepository;

        for (var minute = 0; minute <= 100; minute++)
        {
            TimeCapPicker.Items.Add($"{minute} minutes");
        }

        TimeCapPicker.SelectedIndex = 15;
        TimeCapPicker.SelectedIndexChanged += (_, _) => UpdateTimeCapSummary();

        UpdateTimeCapSummary();
        UpdateNotesLabel();
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
        _notes = savedNote?.Notes ?? string.Empty;
        UpdateNotesLabel();
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
            var selectedMinutes = Math.Clamp(timeCap / 60, 0, 100);
            TimeCapPicker.SelectedIndex = selectedMinutes;
        }

        var savedNote = await _workoutNoteRepository.GetLatestAsync(WorkoutTypes.ForTime, _selectedHeroWod?.UniqueId);
        _notes = savedNote?.Notes
            ?? _selectedHeroWod?.Notes
            ?? string.Empty;

        UpdateTimeCapSummary();
        UpdateNotesLabel();
    }

    private async void OnAddNotesClicked(object? sender, EventArgs e)
    {
        var editedNote = await DisplayPromptAsync(
            "Workout notes",
            "Add the note you want to keep for this FOR TIME setup.",
            accept: "Save",
            cancel: "Cancel",
            placeholder: "Reps, split strategy, scaling...",
            maxLength: 500,
            initialValue: _notes);

        if (editedNote is null)
        {
            return;
        }

        _notes = editedNote.Trim();
        await _workoutNoteRepository.SaveAsync(WorkoutTypes.ForTime, _selectedHeroWod?.UniqueId, _notes);
        UpdateNotesLabel();
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var selectedMinutes = TimeCapPicker.SelectedIndex;
        int? capSeconds = selectedMinutes == 0 ? null : selectedMinutes * 60;

        var request = new WorkoutConfigurationRequest
        {
            Type = WorkoutTypes.ForTime,
            TypeDisplayName = "FOR TIME",
            DurationSeconds = capSeconds ?? 0,
            TimeCapSeconds = capSeconds,
            DurationLabel = $"{selectedMinutes} minutes",
            Notes = _notes,
            HeroWodUniqueId = _selectedHeroWod?.UniqueId,
            HeroWodName = _selectedHeroWod?.Name ?? string.Empty,
            WodDescription = _selectedHeroWod?.WodDescription ?? string.Empty,
            CountsDown = false,
            SupportsRounds = false
        };

        await Navigation.PushAsync(new WorkoutTimerPage(request));
    }

    private void UpdateTimeCapSummary()
    {
        var selectedMinutes = TimeCapPicker.SelectedIndex;
        TimeCapSummaryLabel.Text = selectedMinutes == 0
            ? "Manual stop"
            : $"{selectedMinutes} minutes";
    }

    private void UpdateNotesLabel()
    {
        NotesLabel.Text = string.IsNullOrWhiteSpace(_notes)
            ? "No notes saved yet."
            : _notes;
    }
}
