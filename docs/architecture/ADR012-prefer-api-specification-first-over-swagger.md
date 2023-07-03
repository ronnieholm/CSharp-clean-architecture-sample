# Prefer API specification first over Swagger

## Context

The auto-generated Swagger (OpenAPI) web interface isn't used. API design first
development using Stoplight Studio, following Zalando REST API guidelines, is
preferred.

## Decision

Handcrafting the OpenAPI specification allows use of any part of the OpenAPI
specification. No need to fiddle with Swagger configuration or .NET attributes
and their relations to the generated specification.

## Consequences

While the auto-generated Swagger interface is nice, accidentally changing the
specification is easy. Crafting the specification with Stoplight Studio (possibly
code-generating server-side code from the spec) is more robust.

Instead of going through the Swagger interface, the specification may be loaded
into Postman. Postman can generate stub calls and verify that the service
complies to the specification.