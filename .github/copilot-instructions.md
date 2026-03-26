# Trimex – Copilot Instructions

Trimex is a **.NET 10 MAUI** fitness timer app targeting Android, iOS, macCatalyst, and Windows. It supports five workout timer modes: AMRAP, FOR TIME, EMOM, TABATA, and HERO WODS.

---

## Build

```bash
# Build for a specific target
dotnet build Trimex/Trimex.csproj -f net10.0-android
dotnet build Trimex/Trimex.csproj -f net10.0-ios
dotnet build Trimex/Trimex.csproj -f net10.0-windows10.0.19041.0

# Build all targets
dotnet build Trimex.slnx
```

There are no automated tests and no linter configuration.

---

## Architecture

### Layers

```
Pages  →  Services / Repositories  →  Data (AppDatabase / SQLite)  →  Models
```

- **Pages** – Each workout type has a *configuration page* + *timer page* pair (e.g., `EmomConfigurationPage` + `EmomTimerPage`). No MVVM – all logic lives in code-behind.
- **Services** – `IHeroWodRepository`, `IWorkoutNoteRepository` (repository pattern), `TimerCueService` (audio/haptics), `DatabaseInitializer` (SQLite seed on first run).
- **Data** – `AppDatabase` is a singleton wrapping `SQLiteAsyncConnection` (sqlite-net-pcl). Two tables: `heroWod`, `workoutNote`.
- **Models** – SQLite entities (`HeroWod`, `WorkoutNote`) and configuration request DTOs passed between pages (`WorkoutConfigurationRequest`, `EmomConfigurationRequest`, etc.).
- **Controls** – `CircularProgressView` (custom drawn progress ring), `SlideToEndView` (swipe-to-dismiss).

### Startup sequence

1. `App.xaml.cs` shows `SplashPage` immediately.
2. `DatabaseInitializer.InitializeAsync()` seeds hero WODs if the table is empty.
3. On completion, navigates to `AppShell` (which hosts `HomePage`).

---

## Key conventions

### No MVVM – constructor injection via `IServiceProvider`

Pages are registered in `MauiProgram.cs` and resolved through `IServiceProvider`. Pages **never** hold ViewModel objects; all state is fields on the code-behind class. When a page needs to open another page it calls `_serviceProvider.GetRequiredService<TPage>()`.

```csharp
// MauiProgram.cs registration rules:
// Singleton  → AppShell, HomePage, all repositories/services
// Transient  → every configuration page and timer page
```

### Configuration objects as constructor parameters

Timer pages receive a typed configuration DTO — never query strings or Shell routes:

```csharp
var page = new WorkoutTimerPage(new WorkoutConfigurationRequest { ... });
await Navigation.PushAsync(page);
```

### Navigation is manual stack – no Shell routes

`AppShell` has `FlyoutBehavior="Disabled"`. All navigation uses `Navigation.PushAsync()` / `Navigation.PopAsync()`. Do **not** add Shell route registrations.

### Timer state machine

Every timer page drives a `WorkoutTimerState` enum:

```
Idle → PreCountdown (10 s) → Running → Paused → Completed
```

- Pre-countdown plays 3 beeps + vibrations at 3, 2, 1 seconds via `TimerCueService`.
- Elapsed time is tracked as `_elapsedBeforeCurrentRun + (UtcNow − _currentRunStartedAtUtc)` to survive pauses.
- `IDispatcherTimer` ticks every 200 ms.
- Final warning cues fire at 3, 2, 1 seconds remaining (or before time cap).

### Timer modes at a glance

| Mode | Direction | Progress ring color | Extra |
|------|-----------|--------------------|-----------------------------|
| AMRAP | Countdown | Blue `#423BFF` | Round counter + confetti |
| FOR TIME | Stopwatch up | Red `#FF3B3B` | Optional time cap |
| EMOM | Per-interval countdown | Gold `#FFD166` | Beep on each round start |
| TABATA | Alternating work/rest | Purple `#9B5DE5` | Green=work, Red=rest phases |

### Adding a new timer type

1. Add a value to `WorkoutTypes.cs`.
2. Create a configuration DTO in `Models/`.
3. Add a `*ConfigurationPage` + `*TimerPage` pair in `Pages/`.
4. Register both as **Transient** in `MauiProgram.cs`.
5. Wire up a button in `HomePage.xaml`.

### Dark-only UI

`App.xaml.cs` hardcodes `UserAppTheme = AppTheme.Dark`. Never add light-theme branches.

### Design tokens (always use `StaticResource`, never hardcode hex)

| Resource key | Value | Usage |
|---|---|---|
| `AppBackground` | `#0D0D0D` | Page backgrounds |
| `Surface` | `#161616` | Cards / sheets |
| `AccentBlue` | `#423BFF` | AMRAP |
| `AccentRed` | `#FF3B3B` | FOR TIME |
| `AccentGold` | `#FFD166` | EMOM |
| `AccentGreen` | `#35D07F` | Tabata work phase |
| `AccentPurple` | `#9B5DE5` | Tabata |
| `AccentNeonYellow` | `#CAFD00` | HERO WODS |

All defined in `Trimex/Resources/Styles/Colors.xaml`. Corresponding button/label styles are in `Styles.xaml` (`PrimaryButtonStyle`, `AccentButtonStyle`, `EmomButtonStyle`, `CardBorderStyle`, `TimerValueStyle`, etc.).

### XAML page structure

```xaml
<ContentPage BackgroundColor="{StaticResource AppBackground}">
    <Grid RowDefinitions="Auto,*,Auto">
        <!-- Row 0: Title + subtitle (PageTitleStyle / HeroSubtitleStyle) -->
        <!-- Row 1: Content inside a Border Style="{StaticResource CardBorderStyle}" -->
        <!-- Row 2: Primary action button -->
        <!-- (Optional) Overlay dimmer + bottom sheet appended last for z-order -->
    </Grid>
</ContentPage>
```

### SQLite entities

Decorate with `sqlite-net-pcl` attributes. Column name is the field name by default; use `[Column("_uniqueID")]` only when matching a legacy DB column. Use `[Indexed]` on frequently filtered fields (e.g., `Type`, `HeroWodUniqueId`).

### Hero WOD seeding

`HeroWodDefinitions.GetAll()` is the static seed source. `DatabaseInitializer` inserts these on first run if the `heroWod` table is empty. New built-in WODs belong in `HeroWodDefinitions.cs`, not in migration scripts.

### Audio / haptic cues

Always go through `TimerCueService` — never call platform audio APIs directly. The service handles Android (`ToneGenerator`), iOS (`SystemSound`), and Windows (`Console.Beep`) internally.
