using TasksAPI.Models;

namespace TasksAPI.Data;

public class PinnedTaskRepository : BaseDataAccess, IPinnedTaskRepository
{
    public PinnedTaskRepository(string connectionstring) : base(connectionstring)
    {
    }

    public Task<bool> DeletePinnedTaskAsync(int pinnedTaskId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<PinnedTask>> GetAllPinnedTasksAsync()
    {
        throw new NotImplementedException();
    }

    public Task<PinnedTask> GetPinnedTaskByIdAsync(int pinnedTaskId)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertPinnedTaskAsync(PinnedTask pinnedTask)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdatePinnedTaskAsync(PinnedTask pinnedTask)
    {
        throw new NotImplementedException();
    }
}
