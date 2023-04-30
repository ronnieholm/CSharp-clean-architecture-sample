using Scrum.Infrastructure.Services;
using static Scrum.Application.Stories.Commands;

namespace Scrum.Tests;

public class Tests
{
    public async Task Test()
    {
        var m = new Mediator();
        var r = await m.DispatchAsync(new CreateStoryCommand(Guid.NewGuid(), "Title", "Description"));
    }
}
