# ADR001: Organize types into fewer files based on responsibility

Status: Accepted and active.

## Context

Avoid the management of many smaller .cs files in a deep folder hierarchy.
Instead group a single .cs file types which naturally belong together. For
instance, `story.cs` could hold the aggregate root entity, enumerations, and
value objects which are part of the it.

## Decision

Where applicable, divert from the common single .cs file, single type.

Instead of

```
Domain
  Aggregates
    StoryAggregate
      Story.cs
      Task.cs
      DomainEvents.cs
      Name.cs
      // many small value object and enumerations.
```

prefer

```
Domain
  Aggregates
    Story
      Story.cs
      Task.cs
      DomainEvents.cs
```

The `Story` folder is included so the namespace becomes
`Scrum.Domain.Aggregates.Story`. Otherwise, every entity, value object,
enumeration, and domain event would reside under the same namespace. With more
aggregate this quickly becomes unwieldy.

Without the `Story` folder, instead we could change the namespace in each .cs to
include the aggregate name. We chose not to because most IDEs generate and
verify for the common case of namespace matching file location.

## Consequences

Instead of navigating by file path, prefer to navigate by type name. Then the
size of files become a non-issue.
