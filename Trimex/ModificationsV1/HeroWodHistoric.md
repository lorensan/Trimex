# Objective
To persist the results of Hero WOD executions locally (SQLite).

To allow:
Saving time and notes upon completion.

Viewing the evolution in a graph.

Viewing notes associated with each execution clicking on the graph points.

___

# Data Model (SQLite)

Table: HeroWodHistory

Id (INTEGER, PK, AUTOINCREMENT)
WodName (TEXT, NOT NULL)
Date (TEXT / DATETIME, NOT NULL)
DurationSeconds (INTEGER, NOT NULL)
Notes (TEXT, NULL)

Key Notes

Save time in seconds  simplifies calculations.
Date in UTC or ISO8601.

___

# Flow: Execution Saving
## Trigger
Event: User completes WOD
Action:
Stop timer or swipe the slide button to finish.
Show floating modal

## Modal UI (Post-WOD)

Behavior

Appears after swiping finish button
Background with semi-transparent overlay
Closes automatically after saving

Content

Centered container
Light gray background
Rounded corners

Elements

TextArea:
Placeholder: "Add notes..."
Multiline
Rounded corners
Dark gray text color
Button:
Text: Save
Action: Persist data
Button:
Texto: Cancel
Action: Close modal without saving

## Persistence

On Save:

Insert into HeroWodHistory:
WodName → current name
Date → current timestamp
DurationSeconds → timer value
Notes → textarea content

___

# Flow: Historical Viewing
## Access
Screen: HeroWodsPage.xaml
Action:
Click on WOD → open details
Icon in upper right corner (paper/document)

## Modal UI (History)

Behavior

Floating modal
Closes on click outside
___

# Chart

Type

Line chart or scatter (preferably line with points)

Data

Filter by WODName
Axes
Y-axis (vertical):
Time in minutes
Range: 0 → 120
X-axis (horizontal):
Run number (chronological order)
Incremental index (1, 2, 3...)
Data transformation
DurationSeconds → convert to minutes
Sort by Date ASC
6. Interaction with chart
Each point represents a run
OnPointSelected event:
Get selected item
Show Notes
___

# Panel Notes

Location

Below the graph

Characteristics

Not editable
Rounded corners
Light gray background
Dark gray text
Scroll if overflowed

Content

Display:
Notes
If empty → "No notes available"
___
# Empty states
No history:
Display message: "No history yet"
Do not render graph

___

# Technical considerations
Index by WordName
Avoid recalculating the dataset on each render → cache in ViewModel
Separate:
Data layer (SQLite)
ViewModel
UI
___

# Prompt 


Implement a new feature in my .NET MAUI app:

1. Create a SQLite table "HeroWodHistory" with: 
- Id (int, PK) 
-WodName (string) 
- Date (datetime) 
-DurationSeconds(int) 
- Notes (string)

2. When a WOD finishes: 
- Stop timer 
- Show a modal with: 
- Multiline text input (rounded, gray style) 
- Save button

3. On Save: 
- Insert a new record in HeroWodHistory

4. In HeroWodsPage: 
- Add a top-right icon in WOD detail view 
- On click → open modal with history

5. History modal: 
- Show a line chart: 
- X axis: execution index 
- Y axis: duration (minutes, 0–120) 
- Below chart: 
- Non-editable notes box (rounded, gray) 
- Updates when selecting a point

6. Close modal when clicking outside

Use the application design that is implemented in the app, following the same styling and patterns.