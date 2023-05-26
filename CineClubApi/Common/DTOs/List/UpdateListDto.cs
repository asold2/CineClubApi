using CineClubApi.Common.DTOs.Tag;

namespace CineClubApi.Common.DTOs.List;
public class UpdateListDto
{
    public string  Name { get; set; }
    public bool Public { get; set; }
    public Guid Id { get; set; }

    public Guid CreatorId { get; set; }
    public List<TagDto> Tags{ get; set; }
    public string BackdropPath { get; set; }


}