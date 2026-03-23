namespace Trimex.Models;

public sealed class WorkoutConfigurationRequest
{
    public string Type { get; init; } = string.Empty;
    public string TypeDisplayName { get; init; } = string.Empty;
    public int DurationSeconds { get; init; }
    public int? TimeCapSeconds { get; init; }
    public string DurationLabel { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public int? HeroWodUniqueId { get; init; }
    public string HeroWodName { get; init; } = string.Empty;
    public string WodDescription { get; init; } = string.Empty;
    public bool CountsDown { get; init; }
    public bool SupportsRounds { get; init; }
}
