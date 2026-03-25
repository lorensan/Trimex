#EMOM Function

## Goal
Enable EMOM timer with flexible configuration, allowing users to set work and rest intervals for each minute, and track exercises and rounds effectively.

---

## BASE Structure

The wods will be integrated in the default db that will be in sqlite as part of core of the app. Nothing is required to be modified.

We need to enable the proper button in the HomePage to allow users to create a new wod with EMOM type. 

This button will be colored in a different color to make it more visible and attractive to users. 
When the user clicks on the button, they will be taken to a new page where they can configure their EMOM workout.

## EMOM Function
An EMOM (Every Minute On the Minute) is a time-driven workout format where, at the start of each minute, you perform a predefined amount of work (e.g., reps or a task). Once you finish, the remaining time within that minute becomes rest. When the next minute begins, you start again, either repeating the same exercise or switching to another, for a total set duration.

Key points: the clock dictates the pace, rest is variable based on how fast you complete the work, and failing to finish within the minute indicates the workload is too demanding or pacing is off.

### EMOM Configuration design

Title at center up screen following AMRAP and FOR TIME design, the title text will be "EMOM" and just below a text that be "Every Minute On the Minute".

Below of the Title and description, we will have the configuration section where users can set up their EMOM workout. This section will include:
- **Duration**: An floating box with dropdown items in format: 15 seconds / 20 seconds....increase each 5 seconds and when we reach 1 minute will be each 15 seconds as 01:00, 01:15, 01:30.....until 10 minutes).
- **Rounds**: A floating scrolldown selector with items from 1 to 10. Tapping it opens an overlay list identical in style to the Duration selector. After selection, a summary label below shows rounds and total time in the format "X / MM:SS" — for example, selecting 10 rounds with 30-second duration shows "10 / 05:00".

The full configuration section (title, cards, buttons) is vertically centered on screen, not anchored to the top.

Below of the configuration section, we will have an button in red that put "Death by EMOM" that will guide to another page ```DeathByPage.xaml´´´ we will describe below points.

Just below of previous one a START button in green that will start the EMOM workout and guide to the EMOMTimerPage.xaml where the timer will be implemented.

#### EMOM Timer Page

This page will be similar as the WorkoutTimerPage.xaml but with some differences to accommodate the EMOM format.

Respect the background and colours, icons, bip sounds.... in the title will appear EMOM and before the circle progress system we will have the count of rounds, for example "Round 1 of 10". The circle progress system will be the same as in the WorkoutTimerPage.xaml but instead of showing the time remaining, it will show the time elapsed for the current minute or time configured.

Below the circle progress system, we will have a text that shows the current exercise or task that the user needs to perform for that minute. This text will update at the start of each minute based on the configuration set by the user. (if we have not configured any exercise won't appear nothing, this is only for preconfigured workout or hero wods)


#### Death by EMOM Configuration Page

Title EMOM
Subtitle Death by EMOM

Text with "EACH" and just continue with a selection as the rest of configuration section with a floating box with the time item selection starting at 15 seconds and increasing each 15 seconds until 10 minutes.
This page will be similar as the EMOM Timer Page but we will have only one configuration option that is the time that the timer will whistle and will restart.

The configuration content is vertically centered on screen.

Just below the configuration section we will have the START button.

The timer will be a countdown timer that will start from the selected time and will count down to zero. When it reaches zero, it will whistle and restart the countdown for the next round. The user will continue to perform the exercise until they can no longer complete the work within the time limit, at which point they will stop and record their score based on the number of rounds completed.

The title of the timer will be  Round + number of rounds completed + Total time completed. For example, if the user complete 5 ronds and click on END Session this title will be "Round 5 in 05:00 minutes".

To stop the EMOM we will have an style of slide button that will be in the bottom of the screen with the text "End Session". When the user slide the ball from left to right, the session will over.



