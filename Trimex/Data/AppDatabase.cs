using SQLite;

namespace Trimex.Data;

public sealed class AppDatabase
{
    public AppDatabase()
    {
        var databasePath = Path.Combine(FileSystem.AppDataDirectory, "trimex.db3");

        Connection = new SQLiteAsyncConnection(
            databasePath,
            SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache);
    }

    public SQLiteAsyncConnection Connection { get; }
}
