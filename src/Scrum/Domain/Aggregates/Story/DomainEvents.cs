using Scrum.Domain.Seedwork;

namespace Scrum.Domain.Aggregates.Story;

// TODO: Why the need to repeat all properties? Can't be just pass in the
// updated story or task? Yes in many cases we, but not in the general case. Say
// be had an EmailUpdated event, if we passed in the entire entity, the
// relatonship betwen event and affected fields. In most cases the relationship
// is obvious.
//
// Except for story StoryTaskAdded, we'd have to include StoryId and then the
// entirey StoryTask. For Story remove, we'd only include StoryTaskId and
// StoryId.

public record StoryCreated(
    StoryId Id,
    StoryTitle Title,
    StoryDescription? Description) : DomainEvent;

public record StoryTaskAdded(
    StoryTaskId TaskId,
    StoryId StoryId,
    StoryTaskTitle Title,
    StoryTaskDescription? Description) : DomainEvent;
