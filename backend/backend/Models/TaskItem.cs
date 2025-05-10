using System.ComponentModel.DataAnnotations;

namespace backend.Models;

public class TaskItem
{
    public int Id { get; set; }

    [Required]
    public int ParentId { get; set; }

    [Required]
    public int ChildId { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public string Deadline { get; set; } = string.Empty;

    public string Reward { get; set; } = string.Empty;

    public string Status { get; set; } = "ongoing"; 

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
