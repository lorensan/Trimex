# Implementation Guide: Custom WODs - TRIMEX

This document details the logic to allow users to create, save, and filter their own workouts within the existing architecture.

We have to add a new filter into HeroWodsPage.xaml to filter by custom WODs. This will require a database extension to store user-created WODs and a new UI screen for creating them.

We have to add a new circle button at bottom right of the HeroWodsPage.xaml to open the WOD creation screen. This button will be visible only when the "CUSTOM" filter is selected.

The filter in HeroWodsPae.xaml should be in a grid horizontall that we can slide from left to right, and the "CUSTOM" filter should be the last one on the right. and here add the new Custom filter button.

## 1. Database Extension (SQLite)

To differentiate between Hero WODs (read-only) and custom WODs, we will add a flag or use the same table with a specific category.

### Updated Data Model
```dart
class UserWOD {
final String? id; // Auto-incrementing in SQLite
final String name;
final List<String> sequence; // Saved as a JSON string in SQLite
final String timeTarget;
final bool isCustom; // true for user WODs

UserWOD({
this.id,
required this.name,
required this.sequence,
required this.timeTarget,
this.isCustom = true,
});

}
```

## 2. User Interface: Creation Screen

Based on the **Kinetic Volt** system.

### Screen: Custom WOD Settings

* **WOD Title:** Minimalist styled text input field (`TextField`), bottom border #CCFF00.

* **Sequence Section:**

* Displays added exercises in a vertical list format (queue).

* **Add (+) Button:** A floating or built-in circular button that opens a small text input (max. 50 characters).

* Upon confirmation, the exercise is added to the end of the queue.

* **Time Target:** Numeric or time selector (MM:SS) located at the end of the settings.

* **Final Action:** "SAVE WOD" button that inserts the record into the local database.

## 3. Filtering Logic

On the main screen, the "CUSTOM" filter will execute a query:
`SELECT * FROM hero_wods WHERE is_custom = 1;`

---
*This module allows the TRIMEX community to grow beyond the predefined Heroes.*