using Scrum.Domain.Aggregates.Story;

namespace Scrum.Application.Stories;

public record StoryTaskDto(
    Guid Id,
    string Title,
    string? Description)
{
    public static StoryTaskDto FromDomain(StoryTask st) =>
        new(st.Id,
            st.Title,
              st.Description?.Value);
}

public record StoryDto(
    Guid Id,
    string Title,
    string? Description,
    // TODO: turn in ImmutableArray or ImmutableList
    StoryTaskDto[] Tasks)
{
    public static StoryDto FromDomain(Story s) =>
        new(s.Id,
            s.Title,
              s.Description?.Value,
              s.Tasks.Select(StoryTaskDto.FromDomain).ToArray());
}