namespace CineClubApi.Common.ServiceResults;

public class EntityNotFoundResult : ServiceResult
{
    public EntityNotFoundResult()
    {
        Result = "Entity not found";
        StatusCode = 404;
    }
}