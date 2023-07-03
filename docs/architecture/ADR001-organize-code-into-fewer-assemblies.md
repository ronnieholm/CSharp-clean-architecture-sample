# ADR001: Organize code into fewer assemblies

Status: Accepted and active.

## Context

.NET projects traditionally split each layer of the architecture into its own
assembly. We don't believe this upfront splitting is sufficiently valuable over
creating folders in a single assembly. Don't split the Domain or Application
layers into separate assemblies the need for sharing these assemblies arise,
which rarely happens in practice.

The perceived, theoretical advantage of multiple assemblies may be in that it's
clearer which NuGet dependencies each assembly has. Consulting the project
file, it's obvious that Domain depends (should) only on the .NET framework, that
Application (should) only depend on Domain and .NET framework, and so on.

Another perceived benefit is that is makes it harder for developers to skip
layers. But enforcing this through assemblies boundaries over improved
communication is a mistake.

Many assemblies, each mostly serially dependent on each other also prolongs
build time. So every single developer, on every build would pay the tax for
prematurely splitting into many projects.

Keeping code in fewer assemblies, we make better use of the highly parallel C#
compiler and CPU cores. The C# compiler easily compiles thousands of lines of
code per second. Unless the projects is very large in terms of lines of code, a
single assembly compiles fast once the compiler instance is up and running.

## Decision

Instead of a project structure and dependencies such as

```
src
  Scrum.Domain
  Scrum.Application (depends on Domain)
  Scrum.Infrastructure (depends on Domain + Application)
  Scrum.Web (depends on Domain + Application + Infrastructure)
  Scrum.Cli (depends on Domain + Application + Infrastructure)
tests
  Scrum.UnitTests (depends on Domain + Application)
  Scrum.IntegrationTests (depends on Domain + Application + Infrastructure + Web)
```

whose layout without actual code takes 8-12 seconds to build, prefer

```
src
  Scrum
  Scrum.Tests (depends on Scrum)
```

with build times around 2 seconds.

## Consequences

A single assembly does contains more functionality, but in many cases this is a
non-issue. Instead of separate Cli and Web projects depending on the same
lower-level assemblies, we provide a single assembly which may serve multiple
roles based on command-line parameters.

Organizing a project in a single assembly also makes vertical architecture
possible.
