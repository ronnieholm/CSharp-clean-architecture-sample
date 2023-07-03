# Light vs heavy domain events

Status: Accepted and active.

## Context

Light domain events contain essential fields only: typically Ids and not title,
description, and so on. Because light events contain references, they aren't
self-contained. To a processor, the source domain object, possible the entire
aggregate, with its properties [1] would have to be available to it. But unlike
with EF, we can't query EF's `DbContext` by Id to get at the uncommitted and
changed object.

We could pass the aggregate to the notification handlers, but if presented in an
audit log or used for troubleshooting, what changed may have become implicit
and, if persisted to JSON, the payload potentially large. Also, deletions might
be difficult to express.

## Decision

The decision of switching from

```fsharp
type DomainEvent =
    | StoryCreated of Story
```

to

```fsharp
type StoryCreatedPayload = { StoryId: StoryId; StoryTitle: StoryTitle; StoryDescription: StoryDescription option }
type DomainEvent =
    | StoryCreated of StoryCreatedPayload * DomainEvent.Metadata
```

(Given that these are domain events, we can use domain types. Had this been an
integration event, we should stick to basic types.)

depends on if entities are the application's primary structures (as in C#
maintained by EF and its build-in change tracker) or if domain events are the
primary structures from which entities are reconstituted. After all, few
applications require true event sourcing and the complexities associated with
it.

Publishing a domain event may trigger any number of notification handlers. If
these were to query the store for the updated `Story` or `Task`, to avoid
reading stale state, the store would have to allow reading uncommitted data. The
transaction isn't committed until the end of request processing.

A regular handler must trigger publishing of events. Then changes to entities
become part of the current per request transaction, visible to notification
handlers. Notification handlers may themselves update the database and publish
new events.

If instead we were to store aggregates in a document store, we could
serialize/deserialize the aggregate as a single document and not worry about
change tracking. On the other hand, compared to a relational store, we'd be
limited in the queries we can perform. Also, updates to the domain model become
a similar problem to that of updating events with event sourcing. Database
migrations are simpler, better understood, and has better tooling.

One command isn't limited to emitting a single domain event. Imagine a use case
where a handler adds and removes items to and from a collection, resulting in
multiple events being generated.

## Consequences

Without EF's build-in state tracking, keeping an aggregate in sync with a
relational store can be significant work: imagine writing code to scan an
aggregate for modifications, generating SQL from it. When instead we treat the
store as an immediately updated read-model, we simplified change tracking. We
can do most of what EF is does without its complexities.

[1] https://learn.microsoft.com/en-us/dotnet/architecture/microservices/microservice-ddd-cqrs-patterns/domain-events-design-implementation