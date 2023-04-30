# Scrum clean architecture sample

Proof of concept C# clean architecture sample.

Focus is on applying C# constructs over clueing together libraries and
frameworks. Instead of FluentValidation, MediatR, Entity Framework, we use plain
C# code.

Especially for Entity Framework, we don't want to be limited by features or the
complexities involved in specifying mapping and applying query optimizations.

In the sprit of Casey Muratori and ComputerEnhance, focus is on architecting an
application simple enough to maintain, profile, and optimize now and in the
future.

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