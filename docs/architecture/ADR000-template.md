# ADR000: xxx

Status: Accepted and active.

## Context

As opposed to integratino events Domain events are internal to the domain. It's
therefore okay for those to contain hold domain objects. Integration events
would consist of primitive types.

The downside to domain events holding domain objects is that if we JSON
serialize an event to a SQL table as a means of creating an audit log (we're not
doing event sourcing), the JSON reflects the structure of value object being the
parent of a Value field. If we were to JSON serialize domain event of simple
types, we don't have that issue.

## Decision

We opt _not_ to use primitive types within domain events, though. Itself we
implement methods for custom serializing the domain event to JSON. While more
labories, it's more true to the spirit of domain events being internal to the
domain. It also means internal receivers of domain events will stay within the
domain. The ToJSon serialization takes places in the database layer, not as a
method on the domain event as how to persist domain events is dependent on
persistence.

## Consequences

