namespace Assignment.Infrastructure;

public class User
{
    
    public int Id { get; set; }
    [StringLength(100), Required(ErrorMessage = "Email is required.")]
    public string Name { get; set; }
    [StringLength(100), Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }

    public List<WorkItem> WorkItems { get; set;} = new List<WorkItem>();

    public User(string name, string email) {
        Name = name;
        Email = email;
    }
}
