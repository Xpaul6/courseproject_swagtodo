namespace backend.Models;

public class JoinRequest
{
    public string Code { get; set; } = string.Empty;
    public int ChildId { get; set; }
}