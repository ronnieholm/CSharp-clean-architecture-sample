using Scrum.Domain.Aggregates.Story;

namespace Scrum.Application;

// Marker interface.
public interface IRequest
{    
}

public interface ISystemClock
{
    DateTime UtcNow();
}

public interface IApplicationEnvironment
{
    public ISystemClock SystemClock { get;}
    public IStoryRepository StoryRepository { get; }
}