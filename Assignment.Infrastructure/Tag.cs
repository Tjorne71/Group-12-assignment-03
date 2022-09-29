

namespace Assignment.Infrastructure;

public class Tag
{
    public int Id { get; set; }
    [StringLength(50), Required(ErrorMessage = "Name is required.")]
    public int Name { get; set; }

    public List<WorkItem> WorkItems { get; set;} = new List<WorkItem>();
}
