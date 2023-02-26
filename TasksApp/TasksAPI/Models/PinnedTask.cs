namespace TasksAPI.Models;

public class PinnedTask
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string LabelName { get; set; }
    public string UserId { get; set; }
}
