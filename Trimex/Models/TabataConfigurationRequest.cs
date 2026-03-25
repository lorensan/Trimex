namespace Trimex.Models;

public sealed class TabataConfigurationRequest
{
    public int WorkSeconds { get; init; }
    public int RestSeconds { get; init; }
    public int Rounds { get; init; }
    public string WodDescription { get; init; } = string.Empty;
}
