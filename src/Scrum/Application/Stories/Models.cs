using Scrum.Domain.Aggregates.Story;

namespace Scrum.Application.Stories;

public record StoryDto(
    Guid Id,
    string Name,
    string? Description)
{
    public static StoryDto FromDomain(Story s) =>
        new(s.Id, s.Title, s.Description?.Value);
}

