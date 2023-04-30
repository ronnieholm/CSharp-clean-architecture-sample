using Scrum.Application.Seedwork;
using Scrum.Domain.Aggregates.Story;

// TODO: add cancellation token
// TODO: leave out Id keys from commands.

namespace Scrum.Application.Stories;

public static class Commands
{
    public record CreateStoryCommand(
        Guid Id,
        string Name,
        string? Description
    ) : IRequest;

    public static async Task<Guid> CreateStoryCommandHandlerAsync(IApplicationEnvironment env, CreateStoryCommand cmd)
    {
        var id = new StoryId(cmd.Id);
        var exist = await env.StoryRepository.ExistAsync(id);
        if (exist)
            throw new ConflictException(typeof(Story), cmd.Id);

        // TODO: aggregate errors and throw ValidationException from Application layer.
        var name = new StoryTitle(cmd.Name);
        var description = cmd.Description == null ? null : new StoryDescription(cmd.Description);
        var story = new Story(id, name, description);

        await env.StoryRepository.ApplyEventsAsync(story);
        return cmd.Id;
    }

    public record AddTaskToStoryCommand(
        Guid TaskId,
        Guid StoryId,
        string Name,
        string? Description
    ) : IRequest;

    public static async Task<Guid> AddTaskToStoryHandlerAsync(IApplicationEnvironment env, AddTaskToStoryCommand cmd)
    {
        var storyId = new StoryId(cmd.StoryId);
        var story = await env.StoryRepository.GetByIdAsync(storyId);
        if (story == null)
            throw new NotFoundException(typeof(Story), cmd.StoryId);

        // TODO: aggregate errors
        var taskId = new StoryTaskId(cmd.TaskId);
        var name = new StoryTaskTitle(cmd.Name);
        var description = cmd.Description == null ? null : new StoryTaskDescription(cmd.Description);
        var task = new StoryTask(taskId, name, description);

        story.AddTask(task);
        await env.StoryRepository.ApplyEventsAsync(story);
        return cmd.TaskId;
    }
}