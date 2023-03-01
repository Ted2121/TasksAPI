using System.ComponentModel.DataAnnotations;

namespace TasksAPI.Models;

public class PinnedTask
{
    public int Id { get; set; }
    [Required]
    public string Text { get; set; }
    public string LabelName { get; set; }
    [Required]
    public string UserId { get; set; }
}
