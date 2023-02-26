namespace TasksAPI.Models;

public class SuggestedTask
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int FKSuggestedLabelId { get; set; }
}
