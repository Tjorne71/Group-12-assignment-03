

namespace Assignment.Infrastructure;

public class Tag
{

    [Key]
    public int Id { get; set; }

    [StringLength(50), Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }

    public virtual ICollection<WorkItem> WorkItems { get; set; }

    public Tag (string name)
    {
      Name = name;
      WorkItems = new List<WorkItem>();
    }
}
