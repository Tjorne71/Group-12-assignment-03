namespace Assignment.Infrastructure.Tests;

public class WorkItemRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly WorkItemRepository _repository;
    public WorkItemRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var workItem1 = new WorkItem("WorkItemTest") { Id = 1 };
        var workItem2 = new WorkItem("WorkItemTest2") { Id = 2 };
        var workItem3 = new WorkItem("WorkItemTest3") { Id = 3 };
        workItem2.State = Removed;
        workItem3.State = Active;
        context.WorkItems.AddRange(workItem1, workItem2, workItem3);
        context.SaveChanges();

        _context = context;
        _repository = new WorkItemRepository(_context);
    }

    [Fact]
    public void Delete_WorkItem_with_State_New_returns_Deleted()
    {
        var response = _repository.Delete(1);

        response.Should().Be(Deleted);

    }

    [Fact]
    public void Delete_WorkItem_with_State_Active_returns_Updated()
    {
        var response = _repository.Delete(3);
        response.Should().Be(Updated);
    }

    [Fact]
    public void Delete_WorkItem_with_State_Removed_returns_Conflict()
    {
        var response = _repository.Delete(2);

        response.Should().Be(Conflict);

    }

    [Fact]
    public void Creating_WorkItem_set_State_and_DateTime()
    {
        var workItem = new WorkItemCreateDTO("newTest", null, null, new List<string>());
        Console.WriteLine(workItem);
        var (response, workItemId) = _repository.Create(workItem);
        var workItemDetail = _repository.Find(workItemId);

        response.Should().Be(Created);
        var actual = DateTime.UtcNow;

        actual.Should().BeCloseTo(workItemDetail.Created, precision: TimeSpan.FromSeconds(5));

    }
}
