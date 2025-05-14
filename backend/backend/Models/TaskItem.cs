using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("tasks")]
    public class TaskItem
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("parent_id")]
        [Required]
        public int ParentId { get; set; }
        
        [Column("child_id")]
        [Required]
        public int ChildId { get; set; }

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
}