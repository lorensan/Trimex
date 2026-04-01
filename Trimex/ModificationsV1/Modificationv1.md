# Objetive
the main objetive is capable the solution with the lattest modification and feature described below. Using the techniques implemented and respecting the main functionality that's working currently.

## Modifications
### Sounds
In the current version of the solution, the sound is get from the system and we just created three sounds inside the following folder:
Resources/Sounds/
We need to use those sounds instead of the system sounds, and we need to create a new function that will be responsible for playing the sound, this function will be called PlaySound and it will receive the name of the sound as a parameter, and it will play the sound that is in the Resources/Sounds/ folder.
beep1.mp3 is a single sound and will be used for notify during the last 5 seconds of a timer.
beepEnd.mp3 is a single sound and will be used for notify when the timer ends.
beepStart.mp3 is a single sound and will be used for notify when the timer starts.

This chnages will be implemented in all timers, including the pomodoro timer, the short break timer and the long break timer.

### Changes in #WorkoutTimerPage.xaml
When an AMRAP is selected in the workout timer page, it will appear an button (already implemented), this button contains the number of rounds each click, and the counter will be increased to have the number. This button must be circular in Gray color and the text and label must be #CAFD00.

### Changes in #MainPage.xaml
We need to create at the bottom of the page a new label link to show the application information, for that the main proposal is have a label on the bottom. The label text will be "Application with love by Trimex", and when the user click on it, it will open a new page with the information of the application, such as the version, the author, the license, etc. The label must be in the center of the page and have a font size of 12 and a color of #CAFD00. The page to show will be a floating windows with circular borders and following the same colours, this windows will be closed when click again out of the box.

# Changes in #HeroWodsPage.xaml
We need to change the function to remove a wod, currently if we want to remove a custom workout we need to keep clicked a workout in this view, but we need to change that. Remove that function and include the function to remove when a CUSTOM wod is selected, in the wod information will appear an icon close the history button and this will be only visible por custom wods.

# Changes in #CustomWodCreationPage.xaml
When the user is creating a new custom wod, we need to add a desplegable option to be able to the user select if the wod is a ForTime, EMOM, AMRAP, Tabata and save this information to be able to filter it.
