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

public class StoryTaskName : ValueObject
{
    public const int MaxLength = 50;
    public string Value { get; }

    public StoryTaskName(string value, string field = nameof(StoryTaskName))
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ValidationException(field, "Must not be empty or whitespace.");
        if (value.Length > MaxLength)
            throw new ValidationException(field, $"Length {value.Length} exceeded maximum length {MaxLength}.");
        Value = value;
    }

    public static explicit operator StoryTaskName(string v) => new(v);
    public static implicit operator string(StoryTaskName v) => v.Value;    
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
    public StoryTaskName Name { get; }
    public StoryTaskDescription? Description { get; }

    public StoryTask(StoryTaskId id, StoryTaskName name, StoryTaskDescription? description) : base(id)
    {
        Name = name;
        Description = description;
    }
}
