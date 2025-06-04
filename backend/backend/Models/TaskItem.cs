using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

[Table("tasks")]
public class TaskItem
{
    [Key]
    [Column("id")]
    public int TaskId { get; set; }

    [Column("parent_id")]
    [Required]
    public int ParentId { get; set; }

    [Column("child_id")]
    [Required]
    public int ChildId { get; set; }

    [Column("task_list_id")]
    [Required]
    public int TaskListId { get; set; }

    [Column("description")]
    [Required]
    public string Description { get; set; } = string.Empty;

    [Column("deadline")]
    public DateTime? Deadline { get; set; }

    [Column("reward")]
    public string? Reward { get; set; }

    [Column("status")]
    [Required]
    public string Status { get; set; } = "ongoing";

    [Column("created_at")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
