using TasksAPI.Models;

namespace TasksAPI.Data;

public class SuggestedLabelRepository : BaseDataAccess, ISuggestedLabelRepository
{
    public SuggestedLabelRepository(string connectionstring) : base(connectionstring)
    {
    }

    public Task<bool> DeleteSuggestedLabelAsync(int suggestedLabelId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<SuggestedLabel>> GetAllSuggestedLabelsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<SuggestedLabel> GetSuggestedLabelByIdAsync(int suggestedLabelId)
    {
        throw new NotImplementedException();
    }

    public Task<int> InsertSuggestedLabelAsync(SuggestedLabel suggestedLabel)
    {
        throw new NotImplementedException();
    }

    public Task<bool> UpdateSuggestedLabelAsync(SuggestedLabel suggestedLabel)
    {
        throw new NotImplementedException();
    }
}
