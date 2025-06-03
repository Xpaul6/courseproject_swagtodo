using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("task_lists")]
    public class TaskList
    {
        [Key][Column("id")] public int Id { get; set; }

        [Column("parent_id")]
        [Required]
        public int ParentId { get; set; }

        [Column("child_id")]
        [Required]
        public int ChildId { get; set; }

        [Column("title")]
        [Required]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [Column("created_at")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}