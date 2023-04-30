using System.Data.SQLite;
using System.Diagnostics;
using Scrum.Application;
using Scrum.Domain.Aggregates.Story;

namespace Scrum.Infrastructure.Persistence;

public class StoryRepository : IStoryRepository
{
    readonly IApplicationEnvironment _env;
    readonly SQLiteTransaction _tx;
    readonly SQLiteConnection _con;

    public StoryRepository(IApplicationEnvironment env, SQLiteTransaction tx, SQLiteConnection con)
    {
        _env = env;
        _tx = tx;
        _con = con;
    }

    public Task<bool> ExistAsync(StoryId id)
    {
        throw new NotImplementedException();
    }

    public Task<Story> GetByIdAsync(StoryId id)
    {
        throw new NotImplementedException();
    }

    public async Task ApplyEventsAsync(Story story)
    {
        // TODO: persist domain event itself as JSON.
        var now = _env.SystemClock.UtcNow();

        // TODO: should we store domain events as queue? Would make removing items easier and there is an FIFO order to events.
        foreach (var evt in story.DomainEvents)
        {
            switch (evt)
            {
                case StoryCreated sc:
                {
                    using var cmd = new SQLiteCommand("insert into stories (id, title, description, created_at) values (@id, @title, @description, @createdAt)", _con, _tx);
                    cmd.Parameters.AddWithValue("@id", sc.Id.Value.ToString());
                    cmd.Parameters.AddWithValue("@title", sc.Name); // TODO: rename to title or name
                    cmd.Parameters.AddWithValue("@description", sc.Description);
                    cmd.Parameters.AddWithValue("@created_at", now);
                    var rows = await cmd.ExecuteNonQueryAsync();                
                    Debug.Assert(rows == 1);
                    break;                    
                }
                case StoryTaskAdded sta:
                {
                    using var cmd = new SQLiteCommand("insert into tasks (id, story_id, title, description, created_at) values (@id, @storyId, @title, @description, @createdAt)", _con, _tx);
                    cmd.Parameters.AddWithValue("@id", sta.TaskId.Value.ToString());
                    cmd.Parameters.AddWithValue("@story_id", sta.StoryId.Value.ToString());
                    cmd.Parameters.AddWithValue("@title", sta.Name); // TODO: rename to title or name
                    cmd.Parameters.AddWithValue("@description", sta.Description);
                    cmd.Parameters.AddWithValue("@created_at", now);
                    var rows = await cmd.ExecuteNonQueryAsync();                
                    Debug.Assert(rows == 1);
                    break;
                }
                default:
                    throw new NotImplementedException();
            }
        }

        throw new NotImplementedException();
    }
}