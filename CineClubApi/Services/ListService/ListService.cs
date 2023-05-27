using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.Helpers;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;
using CineClubApi.Repositories.ListTagsRepository;
using CineClubApi.Services.TmdbGenre;
using CineClubApi.Services.TMDBLibService;
using CineClubApi.Services.TMDBLibService.Actor;

namespace CineClubApi.Services.ListService;

public abstract class ListService
{
    protected readonly IListRepository _listRepository;
    protected readonly IUserRepository _userRepository;
    protected readonly ITagRepository _tagRepository;
    protected readonly IMapper _mapper;
    protected readonly IPaginator _paginator;
    protected ITMDBMovieService _movieService;
    protected ITMDBPeopleService _peopleService;
    protected ITMDBGenreService _genreService;
    
    public ListService(IListRepository listRepository,
        IUserRepository userRepository,
        IMapper mapper,
        ITagRepository tagRepository,
        IPaginator paginator,
        ITMDBMovieService movieService,
        ITMDBPeopleService peopleService,
        ITMDBGenreService genreService)
    {
        _listRepository = listRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _tagRepository = tagRepository;
        _paginator = paginator;
        _movieService = movieService;
        _peopleService = peopleService;
        _genreService = genreService;
    }



    public async Task<UpdateListDto> AssignImageToList(UpdateListDto listDto)
    {
        var neededList = await _listRepository.GetListWithMovies(listDto.Id);


        if (neededList.MovieDaos.Count == 0)
        {
            return listDto;
        }

        var randomMovie = neededList.MovieDaos.FirstOrDefault();

        var movieFromTmdb = await _movieService.getMovieById(randomMovie.tmdbId);

        listDto.BackdropPath = movieFromTmdb.BackdropPath;
        listDto.MovieName = movieFromTmdb.OriginalTitle;
        return listDto;
    }

}