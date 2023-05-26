using CineClubApi.Common.DTOs.Actor;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.DTOs.Tag;

namespace CineClubApi.Common.DTOs.List;

public class DetailedListDto
{
    public string Name { get; set; }
    public Guid Id { get; set; }
    public bool Public { get; set; }
    public Guid CreatorId { get; set; }
    public List<MoviePersonDto> Top5ActorsFromList { get; set; }

    public List<MovieForListDto> MovieDtos { get; set; } = new List<MovieForListDto>();
    public List<TagForListDto> TagsDtos { get; set; } = new List<TagForListDto>();
}