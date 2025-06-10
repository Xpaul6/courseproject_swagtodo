using System.ComponentModel;

namespace backend.DTOs;
public record RedeemCode(int Id, string Code, int ParentId);