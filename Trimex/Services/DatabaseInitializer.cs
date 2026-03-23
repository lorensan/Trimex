using Trimex.Data;
using Trimex.Models;

namespace Trimex.Services;

public sealed class DatabaseInitializer(AppDatabase database) : IDatabaseInitializer
{
    private bool _initialized;

    public async Task InitializeAsync()
    {
        if (_initialized)
        {
            return;
        }

        await database.Connection.CreateTableAsync<HeroWod>();
        await database.Connection.CreateTableAsync<WorkoutNote>();

        _initialized = true;
    }
}
