using Trimex.Data;
using Trimex.Models;

namespace Trimex.Services;

public sealed class WorkoutNoteRepository(AppDatabase database) : IWorkoutNoteRepository
{
    public async Task<WorkoutNote?> GetLatestAsync(string workoutType, int? heroWodUniqueId)
    {
        return await database.Connection.Table<WorkoutNote>()
            .Where(note => note.WorkoutType == workoutType && note.HeroWodUniqueId == heroWodUniqueId)
            .OrderByDescending(note => note.UpdatedAtUtc)
            .FirstOrDefaultAsync();
    }

    public async Task SaveAsync(string workoutType, int? heroWodUniqueId, string notes)
    {
        var existing = await database.Connection.Table<WorkoutNote>()
            .Where(note => note.WorkoutType == workoutType && note.HeroWodUniqueId == heroWodUniqueId)
            .FirstOrDefaultAsync();

        if (existing is null)
        {
            await database.Connection.InsertAsync(new WorkoutNote
            {
                WorkoutType = workoutType,
                HeroWodUniqueId = heroWodUniqueId,
                Notes = notes,
                UpdatedAtUtc = DateTime.UtcNow
            });

            return;
        }

        existing.Notes = notes;
        existing.UpdatedAtUtc = DateTime.UtcNow;

        await database.Connection.UpdateAsync(existing);
    }
}
