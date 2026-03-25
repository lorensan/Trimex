namespace Trimex.Models;

public sealed class EmomConfigurationRequest
{
    public int IntervalSeconds { get; init; }
    public int Rounds { get; init; }
    public string WodDescription { get; init; } = string.Empty;
}
