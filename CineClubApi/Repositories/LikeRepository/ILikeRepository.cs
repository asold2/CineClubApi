using CineClubApi.Models;

namespace CineClubApi.Repositories.LikeRepository;

public interface ILikeRepository
{
    Task UserLikeList(Like like);
    Task UserUnlikeList(Like like);
    Task<List<Like>> GetAllLikesByUserId(Guid userId);

    Task<bool> UserHasLikedList(Guid userId, Guid listId);

    Task<Like> GetLikeByUserAndList(Guid userId, Guid listId);
}