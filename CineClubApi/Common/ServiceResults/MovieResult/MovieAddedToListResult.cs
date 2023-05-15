namespace CineClubApi.Common.ServiceResults.MovieResult;

public class MovieAddedToListResult : ServiceResult
{
    public MovieAddedToListResult()
    {
        Result = "Movie added to list";
        StatusCode = 200;
    }
}