using SQLite;

namespace Trimex.Models;

[Table("heroWod")]
public sealed class HeroWod
{
    [PrimaryKey, AutoIncrement, Column("_uniqueID")]
    public int UniqueId { get; set; }

    [Indexed]
    public string Type { get; set; } = string.Empty;

    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(20)]
    public string GenderCategory { get; set; } = string.Empty;

    public int? Duration { get; set; }

    public int? TimeCap { get; set; }

    public string WodDescription { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    // ── Display helpers (not stored in DB) ──────────────────────────────────

    [Ignore]
    public string TypeBadge => Type switch
    {
        WorkoutTypes.ForTime => "FOR TIME",
        WorkoutTypes.Emom    => "EMOM",
        WorkoutTypes.Tabata  => "TABATA",
        _                    => Type
    };

    [Ignore]
    public Color TypeBadgeTextColor => Type switch
    {
        WorkoutTypes.ForTime => Color.FromArgb("#4DA6FF"),  // blue
        WorkoutTypes.Amrap   => Color.FromArgb("#FF8C42"),  // orange
        WorkoutTypes.Tabata  => Color.FromArgb("#35D07F"),  // green
        WorkoutTypes.Emom    => Color.FromArgb("#FFFFFF"),  // white
        _                    => Color.FromArgb("#CAFD00")   // yellow (custom + default)
    };

    [Ignore]
    public Color TypeBadgeBgColor => Type switch
    {
        WorkoutTypes.ForTime => Color.FromArgb("#0D1E33"),
        WorkoutTypes.Amrap   => Color.FromArgb("#2E1800"),
        WorkoutTypes.Tabata  => Color.FromArgb("#0D2E1A"),
        WorkoutTypes.Emom    => Color.FromArgb("#2A2A2A"),
        _                    => Color.FromArgb("#1A2900")   // yellow/custom
    };

    [Ignore]
    public string DurationDisplay => Type switch
    {
        WorkoutTypes.Amrap   => Duration.HasValue ? $"{Duration.Value / 60} min AMRAP" : "—",
        WorkoutTypes.ForTime => TimeCap.HasValue  ? $"{TimeCap.Value  / 60} min cap"   : "No time limit",
        _                    => Duration.HasValue ? $"{Duration.Value / 60} min"        : "—"
    };

    [Ignore]
    public IReadOnlyList<string> ExerciseLines =>
        WodDescription.Split('\n', StringSplitOptions.RemoveEmptyEntries);

    public bool IsCustom { get; set; }

    [Ignore]
    public bool IsPendingDelete { get; set; }

    [Ignore]
    public bool CanShowDelete => IsCustom && IsPendingDelete;

    public WorkoutDefinition ToDefinition() =>
        new()
        {
            Type = Type,
            Name = Name,
            Duration = Duration,
            TimeCap = TimeCap,
            WodDescription = WodDescription,
            Notes = Notes
        };
}
