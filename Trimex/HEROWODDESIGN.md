HERO# Design System Specification: High-Performance Kineticism

## 1. Overview & Creative North Star: "The Kinetic Pulse"
This design system is engineered to move. It rejects the static, boxy nature of traditional fitness apps in favor of a "Kinetic Pulse" aesthetic—a high-energy, editorial approach that mirrors the intensity of a premium training session. 

The Creative North Star is **Aggressive Precision**. We achieve this by blending the raw, high-contrast energy of "Neo-Brutalism" with the sophisticated layering of a luxury digital timepiece. By utilizing intentional asymmetry, oversized display typography, and a "No-Line" philosophy, we create a UI that feels less like a database of exercises and more like a high-performance engine.

---

## 2. Colors & Surface Architecture
The palette is built on a foundation of deep obsidian (`#0e0e0e`), punctuated by high-frequency accents that demand action.

### The "No-Line" Rule
**Explicit Instruction:** 1px solid borders for sectioning are strictly prohibited. Boundaries must be defined solely through background color shifts or tonal transitions. To separate a workout card from the feed, place a `surface_container_low` card atop a `surface` background. The contrast in value provides the structure; the lack of lines provides the "premium" feel.

### Surface Hierarchy & Nesting
Treat the UI as a physical stack of carbon-fiber sheets. 
- **Base Layer:** `surface` (#0e0e0e)
- **Secondary Containers:** `surface_container_low` (#131313) for secondary content.
- **Actionable Layers:** `surface_container_high` (#202020) for interactive elements.
- **Peak Importance:** `surface_container_highest` (#262626) for elevated modals or active states.

### The "Glass & Gradient" Rule
To avoid a "flat" dark mode, use Glassmorphism for floating navigation bars or sticky headers. 
- **Effect:** Use `surface` at 70% opacity with a 20px backdrop-blur. 
- **Signature Textures:** Apply a linear gradient from `primary` (#f3ffca) to `primary_container` (#cafd00) at a 135° angle for primary Action Buttons to simulate a glowing, high-energy "pulse."

---

## 3. Typography: Editorial Dominance
We use a dual-typeface system to balance raw aggression with technical readability.

*   **Display & Headlines:** *Space Grotesk*. This is our "Aggressive" voice. Use `display-lg` (3.5rem) for workout milestones or "GO" triggers. The wide apertures and geometric tension of Space Grotesk convey a sense of modern engineering.
*   **Body & Utility:** *Inter*. This is our "Precision" voice. Inter’s high x-height ensures that even at `body-sm` (0.75rem), heart rate data and rep counts are legible during high-intensity movement.

**The Power Scale:** Use extreme contrast. Pair a `display-lg` metric (e.g., "300kg") directly with a `label-sm` unit (e.g., "TOTAL VOLUME") in `on_surface_variant` to create an editorial, "magazine-style" layout.

---

## 4. Elevation & Depth: Tonal Layering
Traditional drop shadows are too "soft" for this brand. We utilize **Tonal Layering** to define space.

*   **The Layering Principle:** Depth is achieved by "stacking." A `surface_container_lowest` card placed on a `surface_container_low` background creates a natural inset look.
*   **Ambient Shadows:** If a floating action button (FAB) requires a shadow, it must be a "Glow Shadow." Use the `primary` color at 15% opacity with a 32px blur, making the element appear to emit light rather than block it.
*   **The "Ghost Border" Fallback:** If a container sits on a background of the same color, use a `1px` stroke of `outline_variant` (#484847) at **15% opacity**. This creates a "barely-there" definition that feels high-end.
*   **Intentional Asymmetry:** Break the grid. Allow hero images of athletes to overlap from a `surface` layer into a `primary_container` header to create a sense of 3D depth and movement.

---

## 5. Components & Interface Patterns

### Buttons
*   **Primary (The Kinetic Button):** Gradient of `primary` to `primary_container`. Radius: `md` (0.75rem). Type: `title-sm` (Bold, All-Caps).
*   **Secondary:** Ghost style. Transparent fill with a `secondary` (#00e3fd) "Ghost Border" at 20% opacity.

### Progress Gauges (Signature Component)
Instead of standard bars, use high-contrast concentric rings using `secondary` and `tertiary`. Use `surface_container_highest` as the track color to ensure the "empty" part of the goal feels like a physical groove in the UI.

### Cards & Lists
*   **The "Zero-Divider" Rule:** Forbid the use of line dividers between list items. Use a `12` (3rem) vertical spacer or a subtle shift from `surface_container_low` to `surface_container_high` to group content.
*   **Rounding:** Apply `lg` (1rem) to all main content cards. This softens the "aggressive" typography and makes the app feel ergonomic.

### Input Fields
*   **State:** Default state uses `surface_container_highest` with no border. On focus, the background remains, but a `primary` "Ghost Border" appears at 40% opacity.

---

## 6. Do's and Don'ts

### Do
*   **Do** use `primary` (#f3ffca) for "Positive Aggression" (Start, Success, Heavy Weight).
*   **Do** use `secondary` (#00e3fd) for "Technical Data" (Heart Rate, GPS, Splits).
*   **Do** use `tertiary` (#ffeea5) for "Warnings/Recovery" (Rest Timers, Deload Weeks).
*   **Do** use massive scale differences. A `3.5rem` headline next to a `0.75rem` label creates the "Premium" look.

### Don't
*   **Don't** use 100% white (#ffffff) for long-form body text; use `on_surface_variant` (#adaaaa) to reduce eye strain in dark mode.
*   **Don't** use "Soft" rounded corners (like 4px). Stick to the `lg` (1rem) or `xl` (1.5rem) values to maintain the "molded" look.
*   **Don't** ever use a solid grey divider line. If you need a break, use a `px` height `surface_container_highest` fill.
*   **Don't** center-align everything. Use left-aligned, asymmetrical layouts to keep the eye moving—embodying the spirit of "The Kinetic Pulse."