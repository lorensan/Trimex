using Trimex.Models;
using Trimex.Services;

namespace Trimex.Pages;

public partial class CustomWodCreationPage : ContentPage
{
    private readonly IHeroWodRepository _heroWodRepository;
    private readonly List<string> _exercises = [];
    private static readonly IReadOnlyList<KeyValuePair<string, string>> WorkoutTypeOptions =
    [
        new(WorkoutTypes.ForTime, "For Time"),
        new(WorkoutTypes.Amrap, "AMRAP"),
        new(WorkoutTypes.Emom, "EMOM"),
        new(WorkoutTypes.Tabata, "Tabata")
    ];

    public CustomWodCreationPage(IHeroWodRepository heroWodRepository)
    {
        InitializeComponent();
        _heroWodRepository = heroWodRepository;

        WorkoutTypePicker.ItemsSource = WorkoutTypeOptions.Select(option => option.Value).ToList();
        WorkoutTypePicker.SelectedIndex = 0;
    }

    // ── Exercises queue ───────────────────────────────────────────────────────

    private async void OnAddExerciseTapped(object? sender, TappedEventArgs e)
    {
        var exercise = await DisplayPromptAsync(
            "Add Exercise",
            "Describe the movement (e.g. 21 Pull-Ups, 400m Run).",
            accept: "Add",
            cancel: "Cancel",
            placeholder: "e.g. 21 Pull-Ups",
            maxLength: 50);

        if (string.IsNullOrWhiteSpace(exercise)) return;

        _exercises.Add(exercise.Trim());
        RefreshExercisesList();
    }

    private void RemoveExercise(string exercise)
    {
        _exercises.Remove(exercise);
        RefreshExercisesList();
    }

    private void RefreshExercisesList()
    {
        ExercisesListLayout.Children.Clear();

        for (var i = 0; i < _exercises.Count; i++)
        {
            var exercise = _exercises[i];
            var row = BuildExerciseRow(i + 1, exercise);
            ExercisesListLayout.Children.Add(row);
        }
    }

    private View BuildExerciseRow(int index, string exercise)
    {
        var grid = new Grid
        {
            ColumnDefinitions =
            [
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            ],
            BackgroundColor = Color.FromArgb("#131313"),
            Padding = new Thickness(14, 10)
        };
        ((Layout)grid).HeightRequest = 44;

        var exerciseLabel = new Label
        {
            Text            = $"{index}. {exercise}",
            TextColor       = Color.FromArgb("#B8B8B8"),
            FontSize        = 14,
            FontFamily      = "OpenSansRegular",
            VerticalOptions = LayoutOptions.Center
        };

        var removeButton = new Image
        {
            Source          = "del.png",
            HeightRequest   = 22,
            WidthRequest    = 22,
            Aspect          = Aspect.AspectFit,
            VerticalOptions = LayoutOptions.Center,
            Margin          = new Thickness(12, 0)
        };

        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => RemoveExercise(exercise);
        removeButton.GestureRecognizers.Add(tap);

        Grid.SetColumn(exerciseLabel, 0);
        Grid.SetColumn(removeButton, 1);
        grid.Children.Add(exerciseLabel);
        grid.Children.Add(removeButton);

        var wrapper = new Border
        {
            BackgroundColor = Color.FromArgb("#131313"),
            StrokeShape     = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 10 },
            Stroke          = Colors.Transparent,
            Content         = grid,
            Padding         = new Thickness(0)
        };

        return wrapper;
    }

    // ── Save ─────────────────────────────────────────────────────────────────

    private async void OnSaveWodClicked(object? sender, EventArgs e)
    {
        var name = TitleEntry.Text?.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Missing title", "Please enter a name for your WOD.", "OK");
            return;
        }

        if (_exercises.Count == 0)
        {
            await DisplayAlert("No exercises", "Add at least one exercise to the sequence.", "OK");
            return;
        }

        var selectedType = GetSelectedWorkoutType();
        var totalSeconds = ParseTimeValue();

        var wod = new HeroWod
        {
            Type           = selectedType,
            Name           = name,
            GenderCategory = string.Empty,
            IsCustom       = true,
            Duration       = selectedType == WorkoutTypes.ForTime ? null : totalSeconds,
            TimeCap        = selectedType == WorkoutTypes.ForTime ? totalSeconds : null,
            WodDescription = string.Join('\n', _exercises),
            Notes          = string.Empty
        };

        await _heroWodRepository.InsertAsync(wod);
        await Navigation.PopAsync();
    }

    private string GetSelectedWorkoutType()
    {
        if (WorkoutTypePicker.SelectedIndex < 0 || WorkoutTypePicker.SelectedIndex >= WorkoutTypeOptions.Count)
        {
            return WorkoutTypes.ForTime;
        }

        return WorkoutTypeOptions[WorkoutTypePicker.SelectedIndex].Key;
    }

    private int? ParseTimeValue()
    {
        var minutes = int.TryParse(MinutesEntry.Text, out var m) ? m : 0;
        var seconds = int.TryParse(SecondsEntry.Text, out var s) ? s : 0;
        var total   = minutes * 60 + seconds;
        return total > 0 ? total : null;
    }
}
