using TasksAPI.Models;

namespace TasksAPI.Data;

public interface ISuggestedLabelRepository
{
    Task<int> InsertSuggestedLabelAsync(SuggestedLabel suggestedLabel);
    Task<bool> UpdateSuggestedLabelAsync(SuggestedLabel suggestedLabel);
    Task<bool> DeleteSuggestedLabelAsync(int suggestedLabelId);
    Task<SuggestedLabel> GetSuggestedLabelByIdAsync(int suggestedLabelId);
    Task<IEnumerable<SuggestedLabel>> GetAllSuggestedLabelsAsync();
}
