namespace CineClubApi.Common.Helpers;

public class PaginatedResult<T>
{
    public List<T> Result { get; set; }
    public int TotalPages { get; set; }
}