namespace CineClubApi.Common.ServiceResults.MovieResult;

public class CreatedMovieDaoResult : ServiceResult
{
    public CreatedMovieDaoResult()
    {
        StatusCode = 200;
        Result = "Movie dao created with the TMDB Id.";
    }
}