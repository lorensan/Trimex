using Trimex.Data;
using Trimex.Models;

namespace Trimex.Services;

public sealed class HeroWodRepository(AppDatabase database) : IHeroWodRepository
{
    public async Task<IReadOnlyList<HeroWod>> GetAllAsync()
    {
        return await database.Connection.Table<HeroWod>()
            .OrderBy(wod => wod.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<HeroWod>> GetByTypeAsync(string workoutType)
    {
        return await database.Connection.Table<HeroWod>()
            .Where(wod => wod.Type == workoutType)
            .OrderBy(wod => wod.Name)
            .ToListAsync();
    }

    public async Task<HeroWod?> GetByIdAsync(int uniqueId)
    {
        return await database.Connection.Table<HeroWod>()
            .FirstOrDefaultAsync(wod => wod.UniqueId == uniqueId);
    }

    public async Task<int> CountAsync()
    {
        return await database.Connection.Table<HeroWod>().CountAsync();
    }

    public async Task InsertAllAsync(IEnumerable<HeroWod> wods)
    {
        await database.Connection.InsertAllAsync(wods);
    }

    public async Task InsertAsync(HeroWod wod)
    {
        await database.Connection.InsertAsync(wod);
    }
}
