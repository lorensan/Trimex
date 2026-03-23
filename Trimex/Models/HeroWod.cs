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

    public int? Duration { get; set; }

    public int? TimeCap { get; set; }

    public string WodDescription { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

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
