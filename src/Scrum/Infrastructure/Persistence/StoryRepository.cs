using System.ComponentModel.Design.Serialization;
using System.Data.SQLite;
using System.Diagnostics;
using System.Reflection;
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

    public async Task<bool> ExistAsync(StoryId id)
    {
        await using var cmd = new SQLiteCommand("select count(*) from stories where id = @id", _con, _tx);
        cmd.Parameters.AddWithValue("@id", id.Value.ToString());
        var count = (long)(await cmd.ExecuteScalarAsync())!;
        return count switch
        {
            0 => false,
            1 => true,
            _ => throw new InvalidOperationException(count!.ToString())
        };
    }

    public async Task<Story> GetByIdAsync(StoryId id)
    {
        await using var cmd = new SQLiteCommand(
            @"select s.id sid, s.title, s.description sdescription, s.created_at screated_at, s.updated_at supdated_at,
                     t.id tid, t.title, t.description tdescription, t.created_at tcreated_at, t.updated_at tupdated_at
              from stories s
              left join tasks t on s.id == t.story_id
              where s.id = @id", _con, _tx);
        cmd.Parameters.AddWithValue("@id", id.Value.ToString());
        var r = await cmd.ExecuteReaderAsync();

        Story? story = null;
        // read s.id. For each identity s.id, add task.
        while (await r.ReadAsync())
        {
            var title = (string)r["title"];
            var description = r["sdescription"] == DBNull.Value ? null : (string)r["sdescription"];
            var createdAt = DateTime.Parse((string)r["screated_at"]);
            var updatedAt = r["updated_at"] == DBNull.Value ? (DateTime?)null : DateTime.Parse((string)r["supdated_at"]);
            var ctor = typeof(Story).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, null, Type.EmptyTypes, null);
            story = (Story)ctor!.Invoke(new object[]
            {
                id,
                new StoryTitle(title),
                (description == null ? null : new StoryDescription(description))!,
                createdAt,
                createdAt, // TODO: fix me to UpdatedAt non-nullable
                new List<StoryTask>()
            });
        }

        return story!;
    }

    public async Task ApplyEventsAsync(Story story)
    {
        // TODO: persist domain event itself as JSON.
        var now = _env.SystemClock.UtcNow();

        // TODO: should we store domain events as queue? Would make removing items easier and there is an FIFO order to events.
        while (story.DomainEvents.Count > 0)
        {
            var evt = story.DomainEvents.Dequeue();
            switch (evt)
            {
                case StoryCreated sc:
                {
                    await using var cmd = new SQLiteCommand("insert into stories (id, title, description, created_at) values (@id, @title, @description, @createdAt)", _con, _tx);
                    cmd.Parameters.AddWithValue("@id", sc.Id.Value.ToString());
                    cmd.Parameters.AddWithValue("@title", sc.Title.Value);
                    cmd.Parameters.AddWithValue("@description", sc.Description?.Value);
                    cmd.Parameters.AddWithValue("@createdAt", now);
                    var rows = await cmd.ExecuteNonQueryAsync();                
                    Debug.Assert(rows == 1);
                    break;                    
                }
                case StoryTaskAdded sta:
                {
                    await using var cmd = new SQLiteCommand("insert into tasks (id, story_id, title, description, created_at) values (@id, @storyId, @title, @description, @createdAt)", _con, _tx);
                    cmd.Parameters.AddWithValue("@id", sta.TaskId.Value.ToString());
                    cmd.Parameters.AddWithValue("@storyId", sta.StoryId.Value.ToString());
                    cmd.Parameters.AddWithValue("@title", sta.Title.Value);
                    cmd.Parameters.AddWithValue("@description", sta.Description?.Value);
                    cmd.Parameters.AddWithValue("@createdAt", now);
                    var rows = await cmd.ExecuteNonQueryAsync();                
                    Debug.Assert(rows == 1);
                    break;
                }
                default:
                    throw new NotImplementedException(evt.GetType().Name);
            }
        }
    }
}