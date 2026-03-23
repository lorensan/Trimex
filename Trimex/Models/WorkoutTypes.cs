namespace Trimex.Models;

public static class WorkoutTypes
{
    public const string Amrap = "AMRAP";
    public const string ForTime = "FOR_TIME";
    public const string Emom = "EMOM";
    public const string Tabata = "TABATA";
    public const string HeroWods = "HERO_WODS";

    public static string ToDisplayName(string type) =>
        type switch
        {
            ForTime => "FOR TIME",
            HeroWods => "HERO WODS",
            _ => type
        };
}
