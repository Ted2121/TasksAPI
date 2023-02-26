using TasksAPI.Models;

namespace TasksAPI.Data;

public interface IPinnedTaskRepository
{
    Task<int> InsertPinnedTaskAsync(PinnedTask pinnedTask);
    Task<bool> UpdatePinnedTaskAsync(PinnedTask pinnedTask);
    Task<bool> DeletePinnedTaskAsync(int pinnedTaskId);
    Task<PinnedTask> GetPinnedTaskByIdAsync(int pinnedTaskId);
    Task<IEnumerable<PinnedTask>> GetAllPinnedTasksAsync();
}
