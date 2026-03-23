using SQLite;

namespace Trimex.Models;

[Table("workoutNote")]
public sealed class WorkoutNote
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string WorkoutType { get; set; } = string.Empty;

    public int? HeroWodUniqueId { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTime UpdatedAtUtc { get; set; }
}
