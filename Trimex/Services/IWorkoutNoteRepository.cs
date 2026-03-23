using Trimex.Models;

namespace Trimex.Services;

public interface IWorkoutNoteRepository
{
    Task<WorkoutNote?> GetLatestAsync(string workoutType, int? heroWodUniqueId);
    Task SaveAsync(string workoutType, int? heroWodUniqueId, string notes);
}
