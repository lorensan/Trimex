# Goal
We need to make capable of downloading the database from local and saving it locally. This will allow us to have a local copy of the database for offline use or for backup purposes. And in the case we want load it again. User experienice, if we change of mobile, we will have the possibility to save our own database in a sqlite file and we can upload again to have the lattest workout.

# Implementation
In the page #HeroWodsPage.xaml will be added a new button with the icon  #db.png located in /Resources/Images. This icon will be located in the right top corner of the page, and when we click on it will appear an floating box with circular border with two main button:
- first button will be for downloading the database, the icon will be #downloadDB.png and below the icon we will have a text with "Backup your wod database".
- second button will be for uploading the database, the icon will be #uploadDB.png and below the icon we will have a text with "Restore your wod database".

When the user clicks on the first button, it will trigger a function that will create a backup of the current database and save it locally on the device. The user will be prompted to choose a location to save the backup file.
When the user clicks on the second button, it will trigger a function that will allow the user to select a previously saved backup file from their device. Once selected, the application will restore the database using the selected backup file.

# Technical Details
A new backend class to handle the download and upload full database must be created in /Model/BackupDatabase.cs and must implement an interface in /Model/IBackupDatabase.cs. This class will contain the logic for creating a backup of the database and restoring it from a backup file. It will use SQLite to handle the database operations and will interact with the file system to save and retrieve the backup files.