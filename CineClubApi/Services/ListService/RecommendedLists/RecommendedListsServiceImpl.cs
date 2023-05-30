using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.DTOs.Movies;
using CineClubApi.Common.Helpers;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.ListService.LikedList;
using CineClubApi.Services.ListService.WatchedList;
using CineClubApi.Services.TmdbGenre;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Actor;

namespace CineClubApi.Services.ListService.RecommendedLists;

public class RecommendedListsServiceImpl: ListService, IRecommendedListsService
{
    private readonly ILikedListService _likedListService;
    private readonly IWatchedListService _watchedListService;
    private readonly IListService _listService;



    public RecommendedListsServiceImpl(IListRepository listRepository, IUserRepository userRepository, IMapper mapper, ITagRepository tagRepository, IPaginator paginator, ITMDBMovieService movieService, ITMDBPeopleService peopleService, ITMDBGenreService genreService,
        ILikedListService likedListService, IWatchedListService watchedListService, IListService listService
        ) : base(listRepository, userRepository, mapper, tagRepository, paginator, movieService, peopleService, genreService)
    {
        _likedListService = likedListService;
        _watchedListService = watchedListService;
        _listService = listService;
    }

    public async Task<DetailedListDto> GetListOfRecommendedMoviesForUser(Guid userId, int page, int start, int end)
    {
        var watchedList = await _likedListService.GetUsersLikedList(userId);
        var likedList = await _watchedListService.GetUsersWatchedList(userId);

        var watchedListWithMovies = await _listService.GetListsById(watchedList.Id);
        var likedListWithMovies = await _listService.GetListsById(likedList.Id);


        if (watchedListWithMovies.MovieDtos.Count == 0 && likedListWithMovies.MovieDtos.Count == 0)
        {
            return new DetailedListDto();
        }

        // var recommendedMovies = new List<MovieForListDto>();
        var listToRet = new DetailedListDto
        {
            Name = "Recommendations",
            Id = Guid.Empty,
            CreatorId = Guid.Empty,
            Public = true,
            Top5ActorsFromList = null,
            TagsDtos = null,
        };
        
        
        foreach (var movie in watchedListWithMovies.MovieDtos)
        {
            var similarMovies = await _genreService.GetMoviesByGenre(movie.GenreIds, page, start, end);

            foreach (var similarMovie in similarMovies.Movies)
            {
                // Exclude movies already in liked and watched lists
                if (!watchedListWithMovies.MovieDtos.Any(m => m.Id == similarMovie.Id) &&
                    !listToRet.MovieDtos.Any(m => m.Id == similarMovie.Id))
                {
                    // recommendedMovies.Add(similarMovie);
                    listToRet.MovieDtos.Add(similarMovie);
                }
            }
        }

        foreach (var movie in likedListWithMovies.MovieDtos)
        {
            var similarMovies = await _genreService.GetMoviesByGenre(movie.GenreIds, page, start, end);

            foreach (var similarMovie in similarMovies.Movies)
            {
                // Exclude movies already in liked and watched lists
                if (!watchedListWithMovies.MovieDtos.Any(m => m.Id == similarMovie.Id) &&
                    !listToRet.MovieDtos.Any(m => m.Id == similarMovie.Id))
                {
                    // recommendedMovies.Add(similarMovie);
                    listToRet.MovieDtos.Add(similarMovie);

                }
            }
        }

        // var paginatedList = await _paginator.PaginateListOfMovieDtos(recommendedMovies, page, start, end);

        var listOfMovies = new List<MovieForListDto>();
        
        for(int i=0; i<20; i++){
            listOfMovies.Add(listToRet.MovieDtos[i]);
        }

        listToRet.MovieDtos = listOfMovies;
        Console.WriteLine(listToRet.MovieDtos.Count);
        
        return listToRet;
    }

}