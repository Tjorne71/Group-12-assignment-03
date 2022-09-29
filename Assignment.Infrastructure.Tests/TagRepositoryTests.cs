namespace Assignment.Infrastructure.Tests;

public class TagRepositoryTests
{
    private readonly KanbanContext _context;
    private readonly TagRepository _repository;
    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var tagItem1 = new Tag("TagTest") { Id = 1 };
        var tagItem2 = new Tag("TagTest2") { Id = 2 };
        var tagItem3 = new Tag("TagTest3") { Id = 3 };
        var tagItem4 = new Tag("TagTest4") { Id = 4 };
        tagItem4.WorkItems.Add(new WorkItem("WorkItemTest"));
        context.Tags.AddRange(tagItem1, tagItem2, tagItem3,tagItem4);
        context.SaveChanges();

        _context = context;
        _repository = new TagRepository(_context);
    }

    [Fact]
    public void Creating_Existing_Tag_Returns_ConFlict()
    {
        var (response, tagId) = _repository.Create(new TagCreateDTO("TagTest"));

        response.Should().Be(Conflict);
    }

    [Fact]
    public void Tag_Assignet_To_WorkOut_Cant_Be_Deleted_Without_Force()
    {
        var response = _repository.Delete(4);
        response.Should().Be(Conflict);
        response = _repository.Delete(4, true);
        response.Should().Be(Deleted);
    }
}
