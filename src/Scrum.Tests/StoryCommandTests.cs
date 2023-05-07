using Scrum.Application.Stories;
using Scrum.Infrastructure.Services;
using Xunit;

namespace Scrum.Tests;

public class StoryCommandTests
{
    private readonly Mediator _mediator;
    
    public StoryCommandTests()
    {
        _mediator = new Mediator();
    }
    
    [Fact]
    public async void CreateStoryTest()
    {
        var cmd = A.CreateStoryCommand();
        var id = await _mediator.DispatchAsync<Guid>(cmd);
        Assert.Equal(cmd.Id, id);

        var s = await _mediator.DispatchAsync<StoryDto>(new Queries.GetStoryByIdQuery(id));
        Assert.Equal(s.Id, cmd.Id);
        Assert.Equal(s.Title, cmd.Title);
        Assert.Equal(s.Description, cmd.Description);
        // TODO: add createdAt and updatedAt. Provide mock time service
    }
    
    [Fact]
    public async void AddTaskToStoryTest()
    {
        var sid = await _mediator.DispatchAsync<Guid>(A.CreateStoryCommand());
        var cmd = A.AddTaskToStoryCommand() with { StoryId = sid };
        var tid = await _mediator.DispatchAsync<Guid>(cmd);

        var cmd1 = A.AddTaskToStoryCommand() with { StoryId = sid };
        var tid1 = await _mediator.DispatchAsync<Guid>(cmd1);
        
        Assert.Equal(cmd.TaskId, tid);
        
        var s = await _mediator.DispatchAsync<StoryDto>(new Queries.GetStoryByIdQuery(sid));
        Assert.Single(s.Tasks);
        var t = s.Tasks[0];
        Assert.Equal(t.Id, cmd.TaskId);
        Assert.Equal(t.Title, cmd.Title);
        Assert.Equal(t.Description, cmd.Description);
    }
}
