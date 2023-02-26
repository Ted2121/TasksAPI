using TasksAPI.Models;

namespace TasksAPI.Data;

public interface ISuggestedTaskRepository
{
    Task<int> InsertSuggestedTaskAsync(SuggestedTask suggestedTask);
    Task<bool> UpdateSuggestedTaskAsync(SuggestedTask suggestedTask);
    Task<bool> DeleteSuggestedTaskAsync(int suggestedTaskId);
    Task<SuggestedTask> GetSuggestedTaskByIdAsync(int suggestedTaskId);
    Task<IEnumerable<SuggestedTask>> GetAllSuggestedTasksAsync();
}
