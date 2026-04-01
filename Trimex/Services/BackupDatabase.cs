using Microsoft.Maui.Storage;
using Trimex.Data;

namespace Trimex.Services;

/// <summary>
/// Implementation of database backup and restore functionality.
/// </summary>
public class BackupDatabase : IBackupDatabase
{
    private readonly AppDatabase _appDatabase;
    private bool _isBackupOrRestoreInProgress;

    public BackupDatabase(AppDatabase appDatabase)
    {
        _appDatabase = appDatabase;
    }

    public async Task<bool> BackupDatabaseAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_isBackupOrRestoreInProgress)
            {
                await Application.Current?.MainPage?.DisplayAlert("In Progress", "A backup or restore operation is already in progress.", "OK");
                return false;
            }

            _isBackupOrRestoreInProgress = true;

            // Get the current database file path
            var databasePath = Path.Combine(FileSystem.AppDataDirectory, "trimex.db3");

            if (!File.Exists(databasePath))
            {
                await Application.Current?.MainPage?.DisplayAlert("Error", "Database file not found.", "OK");
                return false;
            }

            // Create backup filename with timestamp
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupFileName = $"trimex_backup_{timestamp}.db3";

            // Save to app's cache directory first
            var cacheBackupPath = Path.Combine(FileSystem.CacheDirectory, backupFileName);
            File.Copy(databasePath, cacheBackupPath, overwrite: true);

            // Use Share to let user save the file
            await Share.Default.RequestAsync(new ShareFileRequest
            {
                Title = "Backup Database",
                File = new ShareFile(cacheBackupPath)
            });

            await Application.Current?.MainPage?.DisplayAlert(
                "Success",
                $"Database backup prepared. Choose where to save:\n{backupFileName}",
                "OK");
            
            return true;
        }
        catch (Exception ex)
        {
            await Application.Current?.MainPage?.DisplayAlert(
                "Error",
                $"Backup failed: {ex.Message}",
                "OK");
            return false;
        }
        finally
        {
            _isBackupOrRestoreInProgress = false;
        }
    }

    public async Task<bool> RestoreDatabaseAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_isBackupOrRestoreInProgress)
            {
                await Application.Current?.MainPage?.DisplayAlert("In Progress", "A backup or restore operation is already in progress.", "OK");
                return false;
            }

            _isBackupOrRestoreInProgress = true;

            // Use FilePicker to select a backup file
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Select Database Backup"
            });

            if (result == null)
            {
                // User cancelled
                return false;
            }

            // Validate that the selected file is a database file
            if (!result.FileName.EndsWith(".db3", StringComparison.OrdinalIgnoreCase))
            {
                await Application.Current?.MainPage?.DisplayAlert("Invalid File", "Please select a .db3 database file.", "OK");
                return false;
            }

            try
            {
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "trimex.db3");

                // Backup the current database in case restore fails
                var backupPath = Path.Combine(FileSystem.AppDataDirectory, "trimex_before_restore.db3");
                if (File.Exists(databasePath))
                {
                    File.Copy(databasePath, backupPath, overwrite: true);
                }

                // Copy the selected file to the database location
                using (var sourceStream = File.OpenRead(result.FullPath))
                using (var destStream = File.Create(databasePath))
                {
                    await sourceStream.CopyToAsync(destStream, cancellationToken);
                }

                await Application.Current?.MainPage?.DisplayAlert("Success", "Database restored successfully. Please restart the app to apply changes.", "OK");
                return true;
            }
            catch (Exception restoreEx)
            {
                // Try to restore the backup if restore fails
                var databasePath = Path.Combine(FileSystem.AppDataDirectory, "trimex.db3");
                var backupPath = Path.Combine(FileSystem.AppDataDirectory, "trimex_before_restore.db3");

                if (File.Exists(backupPath))
                {
                    try
                    {
                        File.Copy(backupPath, databasePath, overwrite: true);
                    }
                    catch
                    {
                        // Silently fail
                    }
                }

                await Application.Current?.MainPage?.DisplayAlert("Error", $"Restore failed: {restoreEx.Message}", "OK");
                return false;
            }
        }
        catch (Exception ex)
        {
            await Application.Current?.MainPage?.DisplayAlert("Error", $"Restore failed: {ex.Message}", "OK");
            return false;
        }
        finally
        {
            _isBackupOrRestoreInProgress = false;
        }
    }
}
