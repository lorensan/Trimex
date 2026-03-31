using SQLite;

namespace Trimex.Models;

[Table("heroWodHistory")]
public sealed class HeroWodHistory
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public string WodName { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public int DurationSeconds { get; set; }

    public string Notes { get; set; } = string.Empty;
}
