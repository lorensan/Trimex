using Trimex.Data;
using Trimex.Models;

namespace Trimex.Services;

public sealed class HeroWodRepository(AppDatabase database) : IHeroWodRepository
{
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
}
