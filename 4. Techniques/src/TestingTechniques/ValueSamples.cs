namespace TestingTechniques;

public class ValueSamples
{
    public int Age = 21;

    public User AppUser = new()
    {
        FullName = "Nick Chapsas",
        Age = 21,
        DateOfBirth = new DateOnly(2000, 6, 9)
    };

    public DateOnly DateOfBirth = new(2000, 6, 9);
    public string FullName = "Nick Chapsas";

    internal int InternalSecretNumber = 42;

    public IEnumerable<int> Numbers = new[] { 1, 5, 10, 15 };

    public IEnumerable<User> Users = new[]
    {
        new User
        {
            FullName = "Nick Chapsas",
            Age = 21,
            DateOfBirth = new DateOnly(2000, 6, 9)
        },
        new User
        {
            FullName = "Tom Scott",
            Age = 37,
            DateOfBirth = new DateOnly(1984, 6, 9)
        },
        new User
        {
            FullName = "Steve Mould",
            Age = 43,
            DateOfBirth = new DateOnly(1978, 10, 5)
        }
    };

    public event EventHandler ExampleEvent;

    public virtual void RaiseExampleEvent()
    {
        ExampleEvent(this, EventArgs.Empty);
    }
}