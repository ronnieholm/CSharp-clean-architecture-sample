using Scrum.Domain.Seedwork;

namespace Scrum.Domain.Aggregates.Story;

public class StoryTaskId : ValueObject
{
    public Guid Value { get; }

    public StoryTaskId(Guid value, string field = nameof(StoryTaskId))
    {
        if (value == Guid.Empty)
            throw new ValidationException(field, "Must not be empty.");
        Value = value;
    }

    public static explicit operator StoryTaskId(Guid v) => new(v);
    public static implicit operator Guid(StoryTaskId v) => v.Value;
    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }
}

public class StoryTaskTitle : ValueObject
{
    public const int MaxLength = 50;
    public string Value { get; }

    public StoryTaskTitle(string value, string field = nameof(StoryTaskTitle))
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException(field, "Must not be empty or whitespace.");
        if (value.Length > MaxLength)
            throw new ValidationException(field, $"Length {value.Length} exceeded maximum length {MaxLength}.");
        Value = value;
    }

    public static explicit operator StoryTaskTitle(string v) => new(v);
    public static implicit operator string(StoryTaskTitle v) => v.Value;
    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }
}

public class StoryTaskDescription : ValueObject
{
    public const int MaxLength = 100;
    public string Value { get; }

    public StoryTaskDescription(string value, string field = nameof(StoryTaskDescription))
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException(field, "Must not be empty or whitespace.");
        if (value.Length > MaxLength)
            throw new ValidationException(field, $"Length {value.Length} exceeded maximum length {MaxLength}.");
        Value = value;
    }

    public static explicit operator StoryTaskDescription(string v) => new(v);
    public static implicit operator string(StoryTaskDescription v) => v.Value;
    protected override IEnumerable<object> GetAtomicValues() { yield return Value; }
}

public class StoryTask : Entity<StoryTaskId>
{
    public StoryTaskTitle Title { get; }
    public StoryTaskDescription? Description { get; }

    public StoryTask(StoryTaskId id, StoryTaskTitle title, StoryTaskDescription? description) : base(id)
    {
        Title = title;
        Description = description;
    }
}
