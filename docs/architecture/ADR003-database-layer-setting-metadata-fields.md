# ADR003: Database layer setting metadata fields

Status: Accepted and active.

## Context

Weather we should pass `createdAt` and `updatedat` timestamps into the
constructor of entities and domain events depends on what constituted creation
and update time.

Is it the point in time when the object is created or updated in memory or is it
when the object is saved to the database? The latter makes for simpler domain
logic as the database layer can set metadata.

Including other metadata such as who created or updated a record, the database
layer is still the preferred choice. Otherwise, domain logic would have to know
about and model user. In some domains this is reasonable, in oftentimes users
and authorization is limited to the application layer.

## Decision

We opt for the database layer setting `createdAt` and `updatedAt` on entities
and `OccurredAt` on domain events.

## Consequences

Because entities don't set this metadata, care must be taken to sequence
operations with Applications command handlers. Published domain event handlers
must be able to assume the metadata is up to date. By updating the before
publishing domain events, we ensure an event handler fetching the entity will
retrieve it with metadata previously set by the database layer.
