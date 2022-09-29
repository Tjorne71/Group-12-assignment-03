namespace Assignment.Infrastructure;

public class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions options) : base(options){}
    public DbSet<User> Users => Set<User>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<WorkItem> WorkItems => Set<WorkItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
                    .HasIndex(c => c.Email).IsUnique();
        modelBuilder.Entity<Tag>()
                    .HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<Tag>()
                    .HasIndex(c => c.Name).IsUnique();
        modelBuilder.Entity<WorkItem>()
            .Property(e => e.State)
            .HasConversion(
            v => v.ToString(),
            v => (State)Enum.Parse(typeof(State), v));
    }
}
