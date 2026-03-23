# Trimex - Detailed Implementation (MAUI + StructureJson)

## Goal
Mobile app similar to SmartWOD Timer but improved and without adds. 
- faster UX 
- Flexible system including Hero Wods in each selection and more.
- Click to go, easy use.

---

# Core KEY: StructureJson (Flexbile System)

## Goal
Allow configure any WOD type withou change the code only adding in DB. For that we only want to have in each timer view a list of Hero Wods and once selected one of them, show in a box the wod description and the START button to initiate the timer related with this WOD.

---

## BASE Structure

The wods will be integrated in the default db that will be in sqlite, it will exist a proper table for this proposal, the table for wod will be heroWod, and the structure in code will be as below and sqlite will have the same struct plus _uniqueID 

```json
{
  "type": "AMRAP | EMOM | TABATA | FOR_TIME",
  "name": "name of the wod"
  "duration": 1200,
  "timeCap": 1800,
  "wodDescription": "test about the wod is, reps, exercises...."
  "notes": ""
}
```

---

## Types of Timer (or selection)

All the option will be in the main mobile windows. it will be 5 buttons with each timer. For the momento only we'll work in the timer 1 and 2 called FORTIME and AMRAP but, we are going to have implemented the main windows properly done.

### 1. AMRAP

When we click in AMRAP, the application will go another view, in that view called "AMRAP Configuration" will have a title alligned center and just below a test with the content "Select the number of minutes of your AMRAP" and just below a box with the number integer with the minutes selected, just continue with "minutes". If we click on the box with the minutes will appear an box with an scroll selector with numbers fom 1 to 100 minutes (all selection should be number + minutes) and when we desire and click on a selector, in the previous box will appear the selection chossen.

Just upper border, we will have a label to be able to include notes, in the case we want include notes, it will appear another floating box with a textbox to be able to insert some notes, and click save this notes. and just below of this selection label, it will appear a boton to START the AMRAP.

When click on START a new screen will appear, this screen is the countdown timer and will have the below format:
Title of the view: AMRAP located at top of application
In the middel will appear another title with the minutes selected for example "100 minutes" , and just below will appear the countdown timer, and in the center an icon with the PLAY icon, if we click on PLAY this icon will be replaced by coundown timer from 10 seconds to 1 and when  this countdown end, the icon that will appear is the numbers of countdown, for example in this case from 99:99 minutes to 0. just below this number it will appear a label that text is "click to pause". in the case we click on the numbers, we will pause the countdown timer. and the icon that shoulw appear is PAUSE icon. we can pause or continue whatever we want. To do more easy and shine, that's number will be surronded of a progress bar that will attend the proper percertage of the timer. That's progress bar, I would like to be with easy visible, and colours shinny.

When the timer is ongoing, just below of the timer, we need to include a button what allow us to count how many rounds we have, for that we need to include a little label with the information "how many rounds?" and when we click on this button in the right side of the button a label with the counter of reps that will be increase by one each time we click on the button. the button text will be "ROUND", and when we click this botton appear a type of confetti animation during 2 seconds in the screem (as celebrating a new round has been done).

To stop the watcher, we need to go back to meny, for that in the top-left of the application will appear some arrow pointint to left to attend the go back order.

#### Logic:
- Timer countdown
- User sum rounds manually

---

### 2. FOR TIME

When click in FOR TIME similar to AMRAP, a new view will appear, in the top will appear the Title "FOR TIME", in the midel center scree will appear the below, a text with "As faster as you can", just below a pretty taller than previous text a text with "TIME LIMIT", and just below box with the minutes selected, when click on this box, a floating scroll down selector will appear as similar AMRAP, from 0 minutes to 100 minutes", once selected in the box will appear the selection, and below of the screen will appear the "Add notes" label as AMRAP.

When click on START a new screen will appear, this screen is the countdown timer and will have the below format:
Title of the view: AMRAP located at top of application
In the middel will appear another title with the minutes selected for example "100 minutes" , and just below will appear the cronometer timer, and in the center an icon with the PLAY icon, if we click on PLAY this icon will be replaced by coundown timer from 10 seconds to 1 and when  this countdown end, the icon that will appear is the numbers of countdown, for example in this case will start in 00:00 and will be increasing each seconds to selection limit time. just below this number it will appear a label that text is "click to pause". in the case we click on the numbers, we will pause the countdown timer. and the icon that shoulw appear is PAUSE icon. we can pause or continue whatever we want. To do more easy and shine, that's number will be surronded of a progress bar that will attend the proper percertage of the timer. That's progress bar, I would like to be with easy visible, and colours shinny.



#### LOGIC:
- Ascendent Cronometer
- Stop manual
- TimeCap optional

---

### 3. EMOM

Pending to define


---

### 4. TABATA

Pending to Define

---

### 4. HERO WODS

Pending to Define


---

## PARSER (clave)

```csharp
public class WorkoutDefinition
{
    public string Type { get; set; }
    public int? Duration { get; set; }
    public int? TimeCap { get; set; }
    public int? Rounds { get; set; }
    public List<Interval> Intervals { get; set; }
    public List<Exercise> Exercises { get; set; }
}

public class Interval
{
    public int? Minute { get; set; }
    public int? Work { get; set; }
    public int? Rest { get; set; }
    public string Exercise { get; set; }
}

public class Exercise
{
    public string Name { get; set; }
    public int? Reps { get; set; }
    public string Distance { get; set; }
}
```

---

## 馃攧 Motor din谩mico

```csharp
switch(def.Type)
{
    case "AMRAP":
        StartCountdown(def.Duration);
        break;

    case "FOR_TIME":
        StartStopwatch();
        break;

    case "EMOM":
        StartEmom(def);
        break;

    case "TABATA":
        StartTabata(def);
        break;
}
```

---

# DESIGN (summary)

- Fondo: #0D0D0D
- RED: #FF3B3B
- BLUE: #423BFF
- White text
- Timer bigger

---

# UX KEY

- 2 taps para empezar
- START with countodown from 10 to 0
- STOP Inmediate
- Vibration + audio: when the timer will start a bip will sound each seconds from 3 to 0 (meaning 3 bips and vibration to notice the timer is starting) the same must occurs when the 

---

# RESULT

With this system, we want train with different timer and countdown timer, and be able to add notes etc.... we need to have all structure to don't remake any code when new functionality is comming. for that respect and do the application as most modular you can with the best folder struct as possible.

The main proposal is have the code structured to be able to add whatever feature without additional effort.