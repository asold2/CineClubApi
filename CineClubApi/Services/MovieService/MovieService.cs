using AutoMapper;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.MovieRepository;
using CineClubApi.Services.ListService;
using CineClubApi.Services.ListService.LikedList;
using CineClubApi.Services.ListService.WatchedList;
using CineClubApi.Services.TMDBLibService;

namespace CineClubApi.Services.MovieService;

public class MovieService
{
    protected readonly IMovieRepository _movieRepository;
    protected readonly IListRepository _listRepository;
    protected readonly ITMDBMovieService _tmdbMovieService;
    protected readonly IMapper _mapper;
    protected readonly IListService _listService;

    protected readonly ILikedListService _likedListService;
    protected readonly IWatchedListService _watchedListService;
    
    public MovieService(IMovieRepository movieRepository,
        IListRepository listRepository,
        ITMDBMovieService tmdbMovieService,
        IMapper mapper,
        IListService listService,
        ILikedListService likedListService,
        IWatchedListService watchedListService)
    {
        _movieRepository = movieRepository;
        _listRepository = listRepository;
        _tmdbMovieService = tmdbMovieService;
        _mapper = mapper;
        _listService = listService;
        _likedListService = likedListService;
        _watchedListService = watchedListService;
    }

}