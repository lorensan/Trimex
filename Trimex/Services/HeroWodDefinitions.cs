using Trimex.Models;

namespace Trimex.Services;

public static class HeroWodDefinitions
{
    public static IReadOnlyList<HeroWod> GetAll() =>
    [
        new HeroWod
        {
            Type = WorkoutTypes.ForTime,
            Name = "Murph",
            GenderCategory = "Men",
            TimeCap = 3600,
            WodDescription = "1 mile Run\n100 Pull-Ups\n200 Push-Ups\n300 Air Squats\n1 mile Run\n(With 20 lb vest/body armor)",
            Notes = "Partition pull-ups, push-ups, and squats as needed. Start and finish with the mile run."
        },
        new HeroWod
        {
            Type = WorkoutTypes.ForTime,
            Name = "DT",
            GenderCategory = "Men",
            TimeCap = 1800,
            WodDescription = "5 Rounds:\n12 Deadlifts (155/105 lb)\n9 Hang Power Cleans (155/105 lb)\n6 Push Jerks (155/105 lb)",
            Notes = "Same barbell for all movements. Cycle efficiently and minimize drops."
        },
        new HeroWod
        {
            Type = WorkoutTypes.Amrap,
            Name = "Danny",
            GenderCategory = "Men",
            Duration = 1200,
            WodDescription = "20 Minute AMRAP:\n30 Box Jumps (24/20 in)\n20 Push Press (115/75 lb)\n30 Pull-Ups",
            Notes = "Break up pull-ups early to save grip. Aim for consistent rounds."
        },
        new HeroWod
        {
            Type = WorkoutTypes.ForTime,
            Name = "Kelly",
            GenderCategory = "Men",
            TimeCap = 2700,
            WodDescription = "5 Rounds:\n400m Run\n30 Box Jumps (24/20 in)\n30 Wall Balls (20/14 lb)",
            Notes = "Pace the run to preserve legs for box jumps and wall balls."
        },
        new HeroWod
        {
            Type = WorkoutTypes.Amrap,
            Name = "Nate",
            GenderCategory = "Men",
            Duration = 1200,
            WodDescription = "20 Minute AMRAP:\n2 Muscle-Ups\n4 Handstand Push-Ups\n8 Kettlebell Swings (70/55 lb)",
            Notes = "Sub ring dips + pull-ups for muscle-ups if needed. Steady pace on KB swings."
        },
        new HeroWod
        {
            Type = WorkoutTypes.ForTime,
            Name = "Chad",
            GenderCategory = "Men",
            TimeCap = null,
            WodDescription = "1,000 Step-Ups (20 in box)\n(With 45 lb ruck/vest)",
            Notes = "No time cap. Methodical pace. Break into sets of 50–100. A tribute workout — finish strong."
        },
        new HeroWod
        {
            Type = WorkoutTypes.ForTime,
            Name = "Michael",
            GenderCategory = "Men",
            TimeCap = 2700,
            WodDescription = "3 Rounds:\n800m Run\n50 Back Extensions\n50 Sit-Ups",
            Notes = "Pace the run. Break back extensions into sets to protect the lower back."
        },
        new HeroWod
        {
            Type = WorkoutTypes.Amrap,
            Name = "Cindy",
            GenderCategory = "Women",
            Duration = 1200,
            WodDescription = "20 Minute AMRAP:\n5 Pull-Ups\n10 Push-Ups\n15 Air Squats",
            Notes = "Benchmark WOD. Aim for 20+ rounds. Keep a steady pace throughout."
        },
        new HeroWod
        {
            Type = WorkoutTypes.ForTime,
            Name = "Barbara",
            GenderCategory = "Women",
            TimeCap = 3000,
            WodDescription = "5 Rounds (3 min rest after each):\n20 Pull-Ups\n30 Push-Ups\n40 Sit-Ups\n50 Air Squats",
            Notes = "Rest exactly 3 minutes between rounds. Scale pull-ups if necessary."
        },
        new HeroWod
        {
            Type = WorkoutTypes.ForTime,
            Name = "Annie",
            GenderCategory = "Women",
            TimeCap = 900,
            WodDescription = "50-40-30-20-10:\nDouble-Unders\nSit-Ups",
            Notes = "Benchmark WOD. Sub 2:1 single-unders for double-unders if needed."
        },
    ];
}
