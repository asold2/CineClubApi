﻿using CineClubApi.Common.DTOs.Genre;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Models;

namespace CineClubApi.Services.TMDBLibService.Statistics;

public interface IStatisticsService
{
    Task<List<MovieForListDto>> TopGrossingMoviesOfAllTime();
    Task<List<RatingGenreDto>> GetAverageRatingByGenre();
}