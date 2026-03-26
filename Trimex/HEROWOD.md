# Implementation guid: Hero WODs
# Implementation guid: Hero WODs
# Implementation guid: Hero WODs

This document details the technical and visual logic necessary to implement the Hero WODs system in the TRIMEX application.

we have to enable at first place the Hero WODs as a feature in the app, which will allow users to explore and perform workouts based on famous CrossFit Hero WODs. The implementation will be divided into several key areas: data architecture, user interface design, navigation logic, and a prompt for AI code generation. thats meaning the button on homePage.xaml will be in yellow color and it will guide you to next page defined below.

## 1. Data Architecture (SQLite & Code-First)

To avoid the manual maintenance of an external database, the application will manage its own internal SQLite database.

### Definición del Modelo (Ejemplo en Dart/Flutter)
Modify the current sqlite architecture to include the new design to attend the full functionality
```dart
class HeroWOD {
  final String id;
  final String name;
  final String genderCategory; // 'Men', 'Women'
  final Enum type; // 'AMRAP', 'EMOM', 'For Time', 'Tabata'
  final String description;
  final List<String> exercises;
  final String targetTime;
  final String? lastRecord;
  final int estimatedDurationMinutes;

  HeroWOD({
    required this.id,
    required this.name,
    required this.genderCategory,
    required this.type,
    required this.description,
    required this.exercises,
    required this.targetTime,
    this.lastRecord,
    required this.estimatedDurationMinutes,
  });
}


Insert into SQLite 10 sample hero wods, include it in a class called HeroWodsDefinitions.cs

```

### Synchronization Logic

1. When the app starts, check if the `hero_wods` table is empty.

2. If it is empty, insert the predefined list of WODs (Works of the Day) that resides in the code (HeroWodsDEfinitions.cs hardcoded list).

3. This ensures that the data model is always consistent with the app version.
---

## 2. User Interface (UI) - Visual Specifications

Based on the **Kinetic Volt** design system (Dark Mode, Accent #CCFF00).

### Screen A: Exploration (Browser)
* **Search:** Text field with magnifying glass icon to filter by name.

* **Category Filters (Chips):**

* `Men`, `Women`.

* `Under 1H`, `No Time Limit`.

* **Type Selector (Horizontal Scroll):**

* Buttons for: `AMRAP`, `EMOM`, `Tabata`, `For Time`.

* **Results Grid:**

* `SliverGrid` with 2 columns.

* Cards with dark background (`#0A0A0A`) and rounded corners (`8px`).

* Quick visual indicators (e.g., lightning bolt icon for 'High Speed' or stopwatch for 'Time Bias').

### Screen B: Detail (Floating Modal)
* **Component:** Persistent `BottomSheet` or custom `Dialog`.

* **Header:** Hero name in *Space Grotesk* font (Bold, Italic, #CCFF00).

* **Content:**

* Vertical list of exercises (`ListView`).

* Metrics section: `Time Target` and `Last Record` side by side.

* **Action:** Wide bottom button "GO TO WORKOUT TIMER" with background color `#CCFF00` and black text.

---

## 3. Navigation Logic
When the main button is pressed in the WOD details, the app should navigate to the `WorkoutTimer` screen, passing the following parameters:
* WOD type (to configure the timer's behavior).

* Target time (if applicable).

* WOD name for the final record.


---

## 4. Suggested Prompt for AI
> "Generate the code for a list/grid view of Hero WODs in [Your Framework: Flutter/React]. The UI should follow an aggressive 'Dark Mode' style with neon yellow accents. Implement a dynamic filtering system that combines categories (Men/Women) and training types (AMRAP, EMOM). Clicking on a card should display a bottom sheet with the exercise's technical details and a prominent button to start the timer. The data should come from a local class that syncs with SQLite if the database is empty."