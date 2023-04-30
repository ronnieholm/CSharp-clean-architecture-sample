using Scrum.Application.Seedwork;
using Scrum.Domain.Aggregates.Story;

namespace Scrum.Application.Stories;

public static class Queries
{
    public record GetStoryByIdQuery(Guid Id) : IRequest;

    public static async Task<StoryDto> GetStoryByIdQueryAsync(IApplicationEnvironment env, GetStoryByIdQuery qry)
    {
        var id = new StoryId(qry.Id);
        var story = await env.StoryRepository.GetByIdAsync(id);
        if (story == null)
            throw new NotFoundException(typeof(Story), qry.Id);

        return StoryDto.FromDomain(story);
    }
}