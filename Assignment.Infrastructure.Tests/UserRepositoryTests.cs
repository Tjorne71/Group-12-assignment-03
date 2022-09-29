namespace Assignment.Infrastructure.Tests;

public class UserRepositoryTests : IDisposable
{
    private readonly KanbanContext _context;
    private readonly UserRepository _repository;
    public UserRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();
        var User1 = new User("Test", "test@test.dk") { Id = 1 };
        var User2 = new User("Test2", "test2@test.dk") { Id = 2 };
        var User3 = new User("Test3", "tes3@test.dk") { Id = 3 };
        context.Users.AddRange(User1, User2, User3);
        context.SaveChanges();

        _context = context;
        _repository = new UserRepository(_context);
    }

    [Fact]
    public void Create_given_User_returns_Created_with_UserId()
    {
        var (response, created) = _repository.Create(new UserCreateDTO("Test4", "test4@test.dk"));

        response.Should().Be(Created);

        created.Should().Be(4);
    }

    [Fact]
    public void Create_given_User_with_non_unique_email_returns_Conflict()
    {
        var (response, created) = _repository.Create(new UserCreateDTO("Test5", "test@test.dk"));

        response.Should().Be(Conflict);

    }

    [Fact]
    public void Delete_User_assignt_to_WorkItem_returns_Conflict()
    {
        var (response, created) = _repository.Create(new UserCreateDTO("Test5", "test@test.dk"));

        response.Should().Be(Conflict);

    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
