﻿using CineClubApi.Models;

namespace CineClubApi.Repositories.MovieRepository;

public interface IMovieRepository
{
    Task AddMovieToList(Guid listId, Guid movieDaoId);
    Task<Guid> SaveMovieDao(MovieDao movieDao);
    Task<MovieDao> GetMovieByTmdbId(int tmdnId);

    Task RemoveMovieFromList(Guid listId, Guid neededMovieId);
}