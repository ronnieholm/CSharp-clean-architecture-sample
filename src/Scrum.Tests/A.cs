using Scrum.Application.Stories;

namespace Scrum.Tests;

// Whereas in C# without records, we'd have to create a builder for each type
// with WithX methods for every property, that same boilerplate code is
// generated by the compiler when using records.

// IS combination of the A pattern (http://natpryce.com/articles/000714.html) and the
// ObjectMother pattern (https://martinfowler.com/bliki/ObjectMother.html).

public static class A
{
    // Make these methods rather than readonly properties or NewGuid will be the same for each instance.
    public static Commands.CreateStoryCommand CreateStoryCommand() => new(
        Guid.NewGuid(),
        "Title",
        "Description");

    public static Commands.AddTaskToStoryCommand AddTaskToStoryCommand() => new(
        Guid.NewGuid(),
        Guid.Empty,
        "Title", 
        "Description");
}
