using Trimex.Data;
using Trimex.Models;

namespace Trimex.Services;

public sealed class DatabaseInitializer(AppDatabase database, IHeroWodRepository heroWodRepository) : IDatabaseInitializer
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

        if (await heroWodRepository.CountAsync() == 0)
        {
            await heroWodRepository.InsertAllAsync(HeroWodDefinitions.GetAll());
        }

        _initialized = true;
    }
}
