namespace CineClubApi.Common.ServiceResults.MovieResult;

public class MovieDaoAlreadyExistsResult : ServiceResult
{
    public MovieDaoAlreadyExistsResult()
    {
        StatusCode = 409;
        Result = "Movie with this TMDB Id already exists!";
    }
    
}