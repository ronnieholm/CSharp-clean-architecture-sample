using System.Data.SQLite;
using Scrum.Application;
using Scrum.Domain.Aggregates.Story;
using Scrum.Infrastructure.Persistence;
using static Scrum.Application.Stories.Commands;
using static Scrum.Application.Stories.Queries;

namespace Scrum.Infrastructure.Services;

public class SystemClock : ISystemClock
{
    public DateTime UtcNow() => DateTime.UtcNow;
}

public class ApplicationEnvironment : IApplicationEnvironment, IDisposable
{
    private static readonly ISystemClock Clock;
    private readonly SQLiteConnection _con;
    private readonly SQLiteTransaction _tx;

    static ApplicationEnvironment()
    {
        Clock = new SystemClock();
    }

    public ApplicationEnvironment()
    {
        _con = new SQLiteConnection("URI=file:/home/rh/git/CSharp-clean-architecture-sample/scrum.sqlite");
        _con.Open();
        _tx = _con.BeginTransaction();
        StoryRepository = new StoryRepository(this, _tx, _con);
    }

    public IStoryRepository StoryRepository { get; }
    public ISystemClock SystemClock => Clock;
    public async Task CommitAsync() => await _tx.CommitAsync();
    public async Task RollbackAsync() => await _tx.RollbackAsync();

    public void Dispose()
    {
        _tx.Dispose();
        _con.Dispose();
    }
}

public class Mediator
{
    public async Task<object> DispatchAsync(IRequest request)
    {
        var env = new ApplicationEnvironment();
        object response = request switch
        {
            CreateStoryCommand c => await CreateStoryCommandHandlerAsync(env, c),
            AddTaskToStoryCommand c => await AddTaskToStoryHandlerAsync(env, c),
            GetStoryByIdQuery q => await GetStoryByIdQueryAsync(env, q),
            _ => new NotImplementedException() 
        };

        await env.CommitAsync();
        return response;
    }
}