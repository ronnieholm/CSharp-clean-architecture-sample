using Scrum.Application;
using Scrum.Application.Stories;
using Scrum.Domain.Aggregates.Story;
using static Scrum.Application.Stories.Commands;
using static Scrum.Application.Stories.Queries;

namespace Scrum.Infrastructure.Services;

public class ApplicationEnvironment : IApplicationEnvironment
{
    public IStoryRepository StoryRepository => throw new NotImplementedException();

    public ISystemClock SystemClock => throw new NotImplementedException();
}

public class Mediator
{
    public async Task<object> DispatchAsync(IRequest request)
    {
        var env = new ApplicationEnvironment();

        return request switch
        {
            CreateStoryCommand c => await Commands.CreateStoryCommandHandlerAsync(env, c),
            AddTaskToStoryCommand c => await Commands.AddTaskToStoryHandlerAsync(env, c),
            GetStoryByIdQuery q => await Queries.GetStoryByIdQueryAsync(env, q),
            _ => new NotImplementedException() 
        };

        // switch (request)
        // {
        //     case CreateStoryCommand c1:
        //         return await Commands.CreateStoryCommandHandlerAsync(env, c1);
        //     case AddTaskToStoryCommand c2:
        //         return Commands.AddTaskToStoryHandlerAsync(env, c2);
        //     case GetStoryByIdQuery q1:
        //         return Queries.GetStoryByIdQueryAsync(env, q1);
        //     default:
        //         throw new NotImplementedException();
        // }
    }
}