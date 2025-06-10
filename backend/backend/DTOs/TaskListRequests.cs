namespace backend.DTOs;

public record TaskListCreateRequest(int ParentId, int ChildId, string Title);
public record TaskListUpdateRequest(int ParentId, int ChildId, string Title);