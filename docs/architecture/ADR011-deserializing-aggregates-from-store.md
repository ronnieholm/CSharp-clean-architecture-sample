# Deserializing aggregates from store

## Context

At first sight, it might seem that deserializing an aggregate should always go
through an Application handler. However, deserialization is a fundamentally
different operation from a client asking Core to fetch data. Handler parameters
are intended for external callers, and a handler typically does more than query
the store for a `Story` or a `Task`. The handler could verify permissions or
make external calls which shouldn't happen during deserialization.

## Decision

Deserialization must not happen through a handler as we want to bypass
validation and initialize a `Story` or `Task` record directly. It was the
application itself which serialized the data so it can be assumed valid. Only if
an update to the aggregate's code or a database migration caused stored data to
become invalid will deserialization fail. Such failure is different from that of
a handler. Here we raise an exception as it indicates an unrecoverable bug.

Deserializing using a handler would only work for entities which have been
created (not updated). Entities which have been updated have their `UpdatedAt`
field set; a field we shouldn't be able to set when calling the `create`
function. We can't determine when to call `create` followed by some order of
updates. Not without replaying the event stream.

When creating records directly, we could validate individual field values
through their `create` functions, but not aggregate operations as their order is
unknown. But sticking to how EF deserializes, it sets the fields directly or
through reflection for better performance and because data is assumed to be
valid.

## Consequences

Validating correctness of data potentially changed at rest is the responsibility
of migration code, not deserialization logic.