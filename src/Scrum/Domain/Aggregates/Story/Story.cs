using Scrum.Domain.Seedwork;

namespace Scrum.Domain.Aggregates.Story;

public class StoryId : ValueObject
{
    public Guid Value { get; }

    public StoryId(Guid value, string field = nameof(StoryId))
    {
        if (value == Guid.Empty)
            throw new ValidationException(field, "Must not be empty.");
        Value = value;
    }

    public static explicit operator StoryId(Guid v) => new(v);
    public static implicit operator Guid(StoryId v) => v.Value;
    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }
}

public class StoryName : ValueObject
{
    public const int MaxLength = 50;
    public string Value { get; }

    public StoryName(string value, string field = nameof(StoryName))
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException(field, "Must not be empty or whitespace.");
        if (value.Length > MaxLength)
            throw new ValidationException(field, $"Length {value.Length} exceeded maximum length {MaxLength}.");
        Value = value;
    }

    public static explicit operator StoryName(string v) => new(v);
    public static implicit operator string(StoryName v) => v.Value;    
    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }
}

public class StoryDescription : ValueObject
{
    public const int MaxLength = 100;
    public string Value { get; }

    public StoryDescription(string value, string field = nameof(StoryDescription))
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException(field, "Must not be empty or whitespace.");
        if (value.Length > MaxLength)
            throw new ValidationException(field, $"Length {value.Length} exceeded maximum length {MaxLength}.");
        Value = value;
    }

    public static explicit operator StoryDescription(string v) => new(v);
    public static implicit operator string(StoryDescription v) => v.Value;    
    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }
}

public class Story : AggregateRoot<StoryId>
{
    public StoryName Name { get; }
    public StoryDescription? Description { get; }
    public List<StoryTask> Tasks => _tasks.ToList();

    private readonly ICollection<StoryTask> _tasks = new List<StoryTask>();

    public Story(StoryId id, StoryName name, StoryDescription? description) : base(id)
    {
        Name = name;
        Description = description;
        AddDomainEvent(new StoryCreated(Id, name, description));
    }

    public void AddTask(StoryTask task)
    {
        // TODO: test for duplicate
        // TODO: update tasks collection
        AddDomainEvent(new StoryTaskAdded(task.Id, Id, task.Name, task.Description));
    }
}

public interface IStoryRepository
{
    Task<bool> ExistAsync(StoryId id);
    Task<Story> GetByIdAsync(StoryId id);
    Task ApplyEventsAsync(Story story);
}