using System.ComponentModel;

namespace backend.DTOs;
public record TaskCreateRequest(
    int ParentId,
    int ChildId,
    int TaskListId,
    string Description,
    DateTime? Deadline,
    string? Reward,
    [DefaultValue("ongoing")]
    string Status = "ongoing"
);