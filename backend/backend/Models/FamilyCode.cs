using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    [Table("familycodes")]
	public class FamilyCode
	{
    	[Key]
    	[Column("id")]
    	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    	public int Id { get; set; }

    	[Column("code")]
    	[Required]
    	[MaxLength(8)]
    	public string Code { get; set; } = GenerateUniqueCode();

    	[Column("parent_user_id")] // Явное указание имени столбца из БД
    	[Required]
    	public int ParentId { get; set; }
    
    	[ForeignKey("ParentId")]
    	[InverseProperty("FamilyCodes")]
    	public User Parent { get; set; } = null!;

    	public static string GenerateUniqueCode(int length = 8)
    	{
        	const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
        	var random = new Random();
        	return new string(Enumerable.Repeat(chars, length)
            	.Select(s => s[random.Next(s.Length)]).ToArray());
    	}
	}
}