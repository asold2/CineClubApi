﻿using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;

namespace CineClubApi.Services.MovieService;

public interface IMovieService
{
    Task<Guid> SaveOrGetMovieDao(int tmbdMovieId);
    Task<ServiceResult> AddMovieToList(Guid listId, Guid userId, int tmdbId);
    Task<ServiceResult >DeleteMovieFromList(Guid listId, Guid userId, int tmdbId);

    

    Task<ServiceResult> AddMovieToLikedList(Guid userid, int tmdbId);
    Task<ServiceResult> AddMovieToWatchedList(Guid userid, int tmdbId);
    Task<ServiceResult> RemoveMovieFromWatchedList(Guid userid, int tmdbId);
    Task<ServiceResult> RemoveMovieFromLikedList(Guid userid, int tmdbId);
}