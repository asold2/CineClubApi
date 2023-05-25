﻿using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;

namespace CineClubApi.Services.MovieService;

public interface IMovieService
{
    Task<Guid> SaveOrGetMovieDao(int tmbdMovieId);
    Task<ServiceResult> AddMovieToList(Guid listId, Guid userId, int tmdbId);
    Task<ServiceResult >DeleteMovieFromList(Guid listId, Guid userId, int tmdbId);

    Task<List<UpdateListDto>> GetUsersListsToWhichMovieBelongs(Guid userId, int tmdbId);
    Task<List<UpdateListDto>> GetAllListsMoviebelongsTo(int tmdbId);
    
}