using Trimex.Data;
using Trimex.Models;

namespace Trimex.Services;

public sealed class HeroWodHistoryRepository(AppDatabase database) : IHeroWodHistoryRepository
{
    public async Task<IReadOnlyList<HeroWodHistory>> GetByWodNameAsync(string wodName)
    {
        return await database.Connection.Table<HeroWodHistory>()
            .Where(h => h.WodName == wodName)
            .OrderBy(h => h.Date)
            .ToListAsync();
    }

    public async Task InsertAsync(HeroWodHistory entry)
    {
        await database.Connection.InsertAsync(entry);
    }
}
