

namespace Assignment.Infrastructure;

public class WorkItem
{
    public int Id { get; set; }
    [StringLength(100), Required(ErrorMessage = "Title is required.")]
    public int Title { get; set; }
    [StringLength(int.MaxValue)]
    public string? Description { get; set; }

    [DataType(DataType.Text)]
    public State State { get; set; }
    public User? AssignedTo { get; set; }
    public List<Tag> Tags { get; set; } = new List<Tag>();


}
