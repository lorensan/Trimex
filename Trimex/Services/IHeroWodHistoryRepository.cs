using Trimex.Models;

namespace Trimex.Services;

public interface IHeroWodHistoryRepository
{
    Task<IReadOnlyList<HeroWodHistory>> GetByWodNameAsync(string wodName);
    Task InsertAsync(HeroWodHistory entry);
}
