# Scrum clean architecture sample

Proof of concept C# clean architecture sample.

Focus is on applying C# constructs over cluing together libraries and
frameworks. Instead of FluentValidation, MediatR, Entity Framework, we use plain
C# code.

Especially for Entity Framework, we don't want to be limited by features or the
complexities involved in specifying mapping and applying query optimizations.

In the spirit of Casey Muratori and ComputerEnhance, focus is on architecting an
application simple enough to maintain, profile, and optimize now and in the
future.

(From first principles, create only what's needed instead of using libraries
with more functionality than needed. Use Seedwork to share code rather that
"productizing" common code into assemblies/NuGet packets too early.)

The Scrum domain was chosen because it offers sufficient complexity, yet most 
everyone is familiar with it. In fact, most aspects of the application is
illustrated using only stories and tasks.

## Compiling and running

  $ dotnet build
  $ dotnet test
  $ dotnet run --project src/Scrum
  $ curl file with example requests

## Random

- Following an initial build, build times may be improved by passing to `dotnet`
  the `--no-restore` option:

  $ d clean && time d build                               // 2.6 seconds
  $ d clean && time d build --no-restore                  // 1.9 seconds

  If we know that we haven't updated any NuGet package, `--no-restore` is safe
  (https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-restore).

## References

## Links

- https://github.com/SteveDunn/Vogen
- https://github.com/quick-lint/quick-lint-js/tree/master/docs/architecture
- System.Json.Text custom converter: https://twitter.com/ursenzler/status/1618956836245479425

## Left over

A better file organization would probably involve organizing by vertical slices. Have one file called UserStory with domain, handlers, database, web defined inside it (the ceremony of the C#/clean architecture app is overkill here). Although each file becomes longer, there's fewer files to care about, and code is easier to read top to bottom. Also, recreating the ASP.NET pipeline abstractions inside Application may not be worth it. Just use what ASP.NET provides as in reality it's unlikely most services will have any other hosts but ASP.NET where the pipeline matters.

Example of suggested folder store (maybe even add xUnit et al. tests in there as well. Ship the program with tests of conditionally compile out as in Go/Rust.)

Vertical architecture

Shared.fs
Story.fs (domain + application + ASP.NET handlers + test)
Infrastructure.fs
Program.fs
Horizontal architecture

Domain.fs
Application.fs
Infrastructure.fs
Program.fs

