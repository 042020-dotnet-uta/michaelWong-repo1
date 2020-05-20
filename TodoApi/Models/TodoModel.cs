public class TodoItem
{
    public long Id { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
    public string Secret { get; set; }
    public TodoItemInfo TodoItemInfo { get; set; }
}

public class TodoItemInfo
{
    public long Id { get; set; }
    public string Description { get; set; }
    public long TodoItemId { get; set; }
    public TodoItem TodoItem { get; set; }
}