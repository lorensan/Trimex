using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class AmrapConfigurationPage : ContentPage
{
    private readonly IHeroWodRepository _heroWodRepository;
    private readonly IWorkoutNoteRepository _workoutNoteRepository;
    private readonly List<HeroWod> _heroWods = [];

    private HeroWod? _selectedHeroWod;
    private string _notes = string.Empty;

    public AmrapConfigurationPage(IHeroWodRepository heroWodRepository, IWorkoutNoteRepository workoutNoteRepository)
    {
        InitializeComponent();
        _heroWodRepository = heroWodRepository;
        _workoutNoteRepository = workoutNoteRepository;

        for (var minute = 1; minute <= 100; minute++)
        {
            MinutesPicker.Items.Add($"{minute} minutes");
        }

        MinutesPicker.SelectedIndex = 19;
        MinutesPicker.SelectedIndexChanged += (_, _) => UpdateMinuteSummary();

        UpdateMinuteSummary();
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
        _heroWods.AddRange(await _heroWodRepository.GetByTypeAsync(WorkoutTypes.Amrap));

        HeroWodPicker.Items.Clear();
        HeroWodPicker.Items.Add("Custom AMRAP");

        foreach (var heroWod in _heroWods)
        {
            HeroWodPicker.Items.Add(heroWod.Name);
        }

        HeroWodPicker.SelectedIndex = 0;
        HeroWodEmptyLabel.IsVisible = _heroWods.Count == 0;

        var savedNote = await _workoutNoteRepository.GetLatestAsync(WorkoutTypes.Amrap, null);
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

        if (_selectedHeroWod?.Duration is int duration && duration > 0)
        {
            var selectedMinutes = Math.Clamp(duration / 60, 1, 100);
            MinutesPicker.SelectedIndex = selectedMinutes - 1;
        }

        var savedNote = await _workoutNoteRepository.GetLatestAsync(WorkoutTypes.Amrap, _selectedHeroWod?.UniqueId);
        _notes = savedNote?.Notes
            ?? _selectedHeroWod?.Notes
            ?? string.Empty;

        UpdateMinuteSummary();
        UpdateNotesLabel();
    }

    private async void OnAddNotesClicked(object? sender, EventArgs e)
    {
        var editedNote = await DisplayPromptAsync(
            "Workout notes",
            "Add the note you want to keep for this AMRAP setup.",
            accept: "Save",
            cancel: "Cancel",
            placeholder: "Movement details, scaling, strategy...",
            maxLength: 500,
            initialValue: _notes);

        if (editedNote is null)
        {
            return;
        }

        _notes = editedNote.Trim();
        await _workoutNoteRepository.SaveAsync(WorkoutTypes.Amrap, _selectedHeroWod?.UniqueId, _notes);
        UpdateNotesLabel();
    }

    private async void OnStartClicked(object? sender, EventArgs e)
    {
        var selectedMinutes = MinutesPicker.SelectedIndex + 1;
        var request = new WorkoutConfigurationRequest
        {
            Type = WorkoutTypes.Amrap,
            TypeDisplayName = "AMRAP",
            DurationSeconds = selectedMinutes * 60,
            TimeCapSeconds = selectedMinutes * 60,
            DurationLabel = $"{selectedMinutes} minutes",
            Notes = _notes,
            HeroWodUniqueId = _selectedHeroWod?.UniqueId,
            HeroWodName = _selectedHeroWod?.Name ?? string.Empty,
            WodDescription = _selectedHeroWod?.WodDescription ?? string.Empty,
            CountsDown = true,
            SupportsRounds = true
        };

        await Navigation.PushAsync(new WorkoutTimerPage(request));
    }

    private void UpdateMinuteSummary()
    {
        var selectedMinutes = MinutesPicker.SelectedIndex + 1;
        MinutesSummaryLabel.Text = $"{selectedMinutes} minutes";
    }

    private void UpdateNotesLabel()
    {
        NotesLabel.Text = string.IsNullOrWhiteSpace(_notes)
            ? "No notes saved yet."
            : _notes;
    }
}
