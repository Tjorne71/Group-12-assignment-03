

namespace Assignment.Infrastructure;

public class WorkItem
{
    public WorkItem(string title)
    {
        Title = title;
        State = New;
        Created = DateTime.UtcNow;
    }

    public int Id { get; set; }
    [StringLength(100), Required(ErrorMessage = "Title is required.")]
    public string Title { get; set; }
    [StringLength(int.MaxValue)]
    public string? Description { get; set; }

    [DataType(DataType.Text)]
    public State State { get; set; }

    public DateTime Created { get; set; }

    public DateTime Updated { get; set; }
    public User? AssignedTo { get; set; }
    public ICollection<Tag> Tags { get; set; }
}
