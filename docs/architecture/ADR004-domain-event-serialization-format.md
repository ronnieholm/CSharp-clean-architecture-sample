# ADR004: Domain event serialization format

Status: Accepted and active.

## Context

As opposed to integration events, Domain events are internal to the domain, and
may contain domain objects. Integration events, on the other hand, should
consist of primitive types.

The downside to domain events consisting of domain objects is that its structure
carries over to JSON serialization. We aren't doing event sourcing, but save
events to an audit log. If we were to JSON serialize domain event of simple
types, we didn't have that issue.

## Decision

We opt _not_ to use primitive types within domain events. Instead we implement
methods for custom serializing each domain event to JSON. While more laborious,
it's more true to the spirit of domain events being internal to the domain.

## Consequences

It means internal receivers of domain events stay within the domain. The to JSON
serialization happens places in the database layer, not as a method on the
domain event as how to serialize is dependent on storage.
