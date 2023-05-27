namespace CineClubApi.Common.ServiceResults.MovieResult;

public class MovieRemovedFromList : ServiceResult
{
    public MovieRemovedFromList()
    {
        Result = "Movie removed from list";
        StatusCode = 200;
    }
}