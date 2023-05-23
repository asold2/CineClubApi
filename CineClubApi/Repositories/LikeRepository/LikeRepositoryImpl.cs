using CineClubApi.Common.Interfaces;
using CineClubApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CineClubApi.Repositories.LikeRepository;

public class LikeRepositoryImpl : ILikeRepository
{

    private readonly IApplicationDbContext _applicationDbContext;

    public LikeRepositoryImpl(IApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }
    
    
    public async Task UserLikeList(Like like)
    {
        await _applicationDbContext.Likes.AddAsync(like);
        await _applicationDbContext.SaveChangesAsync();
    }

    public async Task UserUnlikeList(Like like)
    {
        _applicationDbContext.Likes.Remove(like);
        await _applicationDbContext.SaveChangesAsync();

    }

    public async Task<List<Like>> GetAllLikesByUserId(Guid userId)
    {
        var result = await _applicationDbContext.Likes.Where(x => x.UserId == userId).ToListAsync();

        return result;
    }

    public async Task<bool> UserHasLikedList(Guid userId, Guid listId)
    {
        var result = await _applicationDbContext.Likes.AnyAsync(x => x.UserId == userId && x.ListId == listId);

        return result;
    }

    public async Task<Like> GetLikeByUserAndList(Guid userId, Guid listId)
    {
        var neededLike =
            await _applicationDbContext.Likes.FirstOrDefaultAsync(x => x.ListId == listId && x.UserId == userId);

        return neededLike;




    }
}