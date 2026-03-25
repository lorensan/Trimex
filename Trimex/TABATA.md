#TABATA Function

## Goal
Enable TABATA timer with flexible configuration, allowing users to set work and rest intervals and set the numbers of rounds, configuring the timming for rest and for work, and track exercises and rounds effectively.

---

## BASE Structure

The TABATA wods will be integrated in the default db that will be in sqlite as part of core of the app. Nothing is required to be modified.

We need to enable the proper button in the HomePage to allow users to create a new wod with TABATA type. 

This button will be colored in a different color to make it more visible and attractive to users. 
When the user clicks on the button, they will be taken to a new page where they can configure their TABATA workout.

___

## TABATA Function
A Tabata is a high-intensity interval training format consisting of tie configured for example 20 seconds of work followed by time resting for example 10 seconds of rest, repeated during a number of rounds configured for example 8 consecutive rounds (total of 4 minutes).

At the start of each 20-second interval, the user performs an exercise at near-max effort, then fully stops during the 10-second rest. This cycle repeats automatically without variation in timing.

Key points: it is strictly time-driven, intervals are fixed (no variable rest), intensity is meant to be maximal, and the structure is always 20s/10s × 8 rounds unless explicitly modified.
Key points: the clock dictates the pace, rest is variable based on how fast you complete the work, and failing to finish within the minute indicates the workload is too demanding or pacing is off.



### TABATA Configuration design

An AmrapTimerPage.xaml will be created with the title "TABATA" aligend in the center top of the screen, just below a subtitle that text be "Configure your tabata as you like".

Just below the both, we will have the following:

- a box with the number of rounds followed of text ROUNDS, this box if I click on it should appear a floating box with selection items  scrolldown from 1 ... 100
- just below, a box with the time in seconds or minutes as we have configured, and if we click on this box a floating box with selection items scrolldown starting at 5 seconds until 15 minutes but the way to increment will be (5 seconds 10 seconds, until 1 minute, and when reached 1 minute will increase into 30 seconds, for example 1:00, 1:30, 2:00, 2:30... until 15 minutes).

#### TABATA Timer Page

This page will be similar as the WorkoutTimerPage.xaml but with some differences to accommodate the TABATA format.

Respect the background and colours, icons, bip sounds.... in the title will appear TABATA and before the circle progress system we will have the count of rounds, for example "Round 1 of 10".
The circle progress system will be the same as in the WorkoutTimerPage.xaml but instead of showing the time remaining, it will show the time elapsed for the current minute or time configured.
We need to differentiate the active round and rest round, the active round the color of the numbers in countdown should be green and resting should be red.
At beginning the number of round will appear something like this (1/100...100/100) and we will count only round completed not the resting time.

Below the circle progress system, we will have a text that shows the current exercise or task that the user needs to perform for that minute. This text will update at the start of each minute based on the configuration set by the user. (if we have not configured any exercise won't appear nothing, this is only for preconfigured workout or hero wods)

To stop the TABATA we will include an slide format button as we have donde in EMOM to stop the timer.



