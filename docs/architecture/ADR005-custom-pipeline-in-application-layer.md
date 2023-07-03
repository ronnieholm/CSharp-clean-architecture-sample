# ADR005: custom pipeline in application layer

Status: Accepted and active.

## Context

Before and after each command or query is executed by Application, the request
goes passes through a pipeline. The pipeline may be setup with any number of
behaviors (a term borrowed from MediatR), each implementing a cross-cutting
concern. Each behavior may modify the request and/or response as they pass
through it, but more commonly doesn't.

An example of a pipeline of behaviors is:

Logging <-> UnhaltedException <-> Performance <-> Authorization <-> Command/Query handler

First the request travels from left to right, then the response travels from
right to left.

## Decision

Because Application is implementation agnostic, how to log (using .NET logging
or Serilog), is defined by Infrastructure. Inside the Logging behavior for
instance, we log through an application defined interface. Logging may entail
serializing the request to JSON and logging it. This way Azure AppInsights will
contain the complete request in case of failure. It's easy for us to see exactly
what the client sent and replay a failing request.

A pipeline may be implemented in at least three different ways:

1. Directly adding any before and after code to the request dispatcher method.
1. Implementing each behavior as a class with behavior state and a
   reference to the next behavior.
1. Implementing each behavior as a method closing over any behavior state and a
   reference to the next behavior.

Each is a way to implement the decorator pattern. (1) mixes dispatching with
behaviors which may be acceptable for a simple pipeline, (2) is good for
behaviors with significant state and code, (3) is good for short behaviors.

## Consequences

A host such as ASP.NET provides its own pipeline. We could take advantage of its
pipeline and avoid implementing our own in Application. The downside is the
ASP.NET pipeline is host specific. We'd need to map it to each hosting
technology. If we were to create a CLI, exercising Application, the pipeline
wouldn't be available.

We do use the ASP.NET pipeline, but only for ASP.NET specific behaviors: mapping
exception types to HTTP return codes.

If we knew that Application would only ever have a single host, we could use the
ASP.NET pipeline for all behaviors. But implementing a custom pipeline in
Application is fairly simple. ASP.NET or MediatR pipelines may seem like a lot
of work to duplicate. These hook into dependency injection and are runtime
configurable; properties which this specific case doesn't need.
