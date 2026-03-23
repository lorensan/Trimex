namespace Trimex.Models;

public class WorkoutDefinition
{
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int? Duration { get; set; }
    public int? TimeCap { get; set; }
    public int? Rounds { get; set; }
    public string WodDescription { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public List<Interval> Intervals { get; set; } = [];
    public List<Exercise> Exercises { get; set; } = [];
}

public class Interval
{
    public int? Minute { get; set; }
    public int? Work { get; set; }
    public int? Rest { get; set; }
    public string Exercise { get; set; } = string.Empty;
}

public class Exercise
{
    public string Name { get; set; } = string.Empty;
    public int? Reps { get; set; }
    public string Distance { get; set; } = string.Empty;
}
