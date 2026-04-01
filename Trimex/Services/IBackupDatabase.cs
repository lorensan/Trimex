namespace Trimex.Services;

/// <summary>
/// Service for backing up and restoring the SQLite database.
/// Allows users to export database to a file for backup/portability and restore from previously saved backups.
/// </summary>
public interface IBackupDatabase
{
    /// <summary>
    /// Creates a backup of the current database and saves it to a user-selected location.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>True if backup was successful, false if cancelled or failed.</returns>
    Task<bool> BackupDatabaseAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Restores the database from a user-selected backup file.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token for async operations.</param>
    /// <returns>True if restore was successful, false if cancelled or failed.</returns>
    Task<bool> RestoreDatabaseAsync(CancellationToken cancellationToken = default);
}
