namespace CineClubApi.Common.DTOs.List;

public class SimpleUpdateListDto
{
    public string Name { get; set; }
    public Guid  Id { get; set; }
    public bool Public { get; set; }
    public Guid CreatorId { get; set; }
    
    
}