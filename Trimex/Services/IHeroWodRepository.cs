using Trimex.Models;

namespace Trimex.Services;

public interface IHeroWodRepository
{
    Task<IReadOnlyList<HeroWod>> GetAllAsync();
    Task<IReadOnlyList<HeroWod>> GetByTypeAsync(string workoutType);
    Task<HeroWod?> GetByIdAsync(int uniqueId);
    Task<int> CountAsync();
    Task InsertAllAsync(IEnumerable<HeroWod> wods);
    Task InsertAsync(HeroWod wod);
}
