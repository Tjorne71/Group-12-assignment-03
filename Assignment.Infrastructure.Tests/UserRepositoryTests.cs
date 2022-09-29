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

    public void Dispose()
    {
        _context.Dispose();
    }
}
