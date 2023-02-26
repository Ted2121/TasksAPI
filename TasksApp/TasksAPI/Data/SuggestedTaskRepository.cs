using TasksAPI.Models;

namespace TasksAPI.Data;

public class SuggestedTaskRepository : BaseDataAccess, ISuggestedTaskRepository
{
    public SuggestedTaskRepository(string connectionstring) : base(connectionstring)
    {
    }

    public Task<bool> DeleteSuggestedTaskAsync(int suggestedTaskId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SuggestedTask>> GetAllSuggestedTasksAsync()
    {
        throw new NotImplementedException();
    }

    public Task<SuggestedTask> GetSuggestedTaskByIdAsync(int suggestedTaskId)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertSuggestedTaskAsync(SuggestedTask suggestedTask)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateSuggestedTaskAsync(SuggestedTask suggestedTask)
    {
        throw new NotImplementedException();
    }
}
