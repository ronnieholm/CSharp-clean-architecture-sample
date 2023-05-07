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

public class StoryTitle : ValueObject
{
    public const int MaxLength = 50;
    public string Value { get; }

    public StoryTitle(string value, string field = nameof(StoryTitle))
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException(field, "Must not be empty or whitespace.");
        if (value.Length > MaxLength)
            throw new ValidationException(field, $"Length {value.Length} exceeded maximum length {MaxLength}.");
        Value = value;
    }

    public static explicit operator StoryTitle(string v) => new(v);
    public static implicit operator string(StoryTitle v) => v.Value;
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
    public StoryTitle Title { get; }
    public StoryDescription? Description { get; }
    public List<StoryTask> Tasks { get; }

    public Story(StoryId id, StoryTitle title, StoryDescription? description, DateTime createdAt, DateTime updatedAt, List<StoryTask> tasks) : base(id)
    {
        // TODO: verify that if called from anywhere but a repository, then throw exception.
        // Rather than being absolute in design choices and making the ctor private so it can't be called by accident,
        // and incurring the trouble of using reflecting to locate and call it (and possible runtime issues may cause),
        // make it public and check at runtime that it's used as intended. Being absolute, non-pragmatic, is a good
        // source of accidental complexity.
        //
        // This ctor will always have more arguments than the one non-repo clients must use as it includes timestamps.
        Title = title;
        Description = description;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
        Tasks = tasks;
    }

    public Story(StoryId id, StoryTitle title, StoryDescription? description) : base(id)
    {
        Title = title;
        Description = description;
        Tasks = new List<StoryTask>();
        DomainEvents.Enqueue(new StoryCreated(Id, title, description));
    }

    public void AddTask(StoryTask task)
    {
        // TODO: test for duplicate
        // TODO: update tasks collection
        DomainEvents.Enqueue(new StoryTaskAdded(task.Id, Id, task.Title, task.Description));
    }
}

public interface IStoryRepository
{
    Task<bool> ExistAsync(StoryId id);
    Task<Story> GetByIdAsync(StoryId id);
    Task ApplyEventsAsync(Story story);
}
