using Trimex.Models;

namespace Trimex.Services;

public interface IHeroWodRepository
{
    Task<IReadOnlyList<HeroWod>> GetByTypeAsync(string workoutType);
    Task<HeroWod?> GetByIdAsync(int uniqueId);
}
