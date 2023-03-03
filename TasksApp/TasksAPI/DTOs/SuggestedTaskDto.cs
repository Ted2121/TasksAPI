namespace TasksAPI.DTOs;

public class SuggestedTaskDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public int FKSuggestedLabelId { get; set; }
}
