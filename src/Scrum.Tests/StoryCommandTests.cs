using Scrum.Infrastructure.Services;
using Xunit;
using static Scrum.Application.Stories.Commands;

namespace Scrum.Tests;

public class StoryCommandTests
{
    [Fact]
    public async void CreateStoryTest()
    {
        var m = new Mediator();
        var r = await m.DispatchAsync(new CreateStoryCommand(Guid.NewGuid(), "Title", "Description"));
        Assert.True(true);
    }
    
    [Fact]
    public async void AddTaskToStoryTest()
    {
        var m = new Mediator();
        var r = await m.DispatchAsync(new CreateStoryCommand(Guid.NewGuid(), "Title", "Description"));
        r = await m.DispatchAsync(new AddTaskToStoryCommand(Guid.NewGuid(), (Guid)r, "Title", "Description"));
        Assert.True(true);
    }
}
