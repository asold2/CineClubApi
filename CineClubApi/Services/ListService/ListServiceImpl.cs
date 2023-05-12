using AutoMapper;
using CineClubApi.Common.DTOs.List;
using CineClubApi.Common.ServiceResults;
using CineClubApi.Common.ServiceResults.ListResults;
using CineClubApi.Models;
using CineClubApi.Models.Auth;
using CineClubApi.Repositories.AccountRepository;
using CineClubApi.Repositories.ListRepository;

namespace CineClubApi.Services.ListService;

public class ListServiceImpl : IListService
{

    private readonly IListRepository _listRepository;
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public ListServiceImpl(IListRepository listRepository, IUserRepository userRepository, IMapper mapper)
    {
        _listRepository = listRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }
    
    
    public async Task<ServiceResult> CreateNamedList(ListDto listDto)
    {

        var neededUser = await _userRepository.GetUserByRefreshToken(listDto.TokenBody.RefreshToken);

        var list = new List
        {
            Name = listDto.Name,
            Public = listDto.Public,
            Creator  = neededUser,
            CreatorId = neededUser.Id
        };
        
        
        await _listRepository.CreateEntity(list);
        return new CreatedListResult
        {
            Result = "Created new List named: " + list.Name,
            StatusCode = 200
        };
    }

    public async Task<ServiceResult> UpdateListNameOrStatus(UpdateListDto updateListDto)
    {
        var listToUpdate =(List) await _listRepository.GetEntityById(updateListDto.Id);

        if (listToUpdate == null)
        {
            return new EntityNotFoundResult();
        }

        listToUpdate.Name = updateListDto.Name;
        listToUpdate.Public = updateListDto.Public;
        
        await _listRepository.UpdateEntity(listToUpdate);

        return new ListSuccessfullyUpdateResult();
    }

    public async Task<IList<UpdateListDto>> GetListsByUserId(string tokenBody)
    {
        var neededUser = await _userRepository.GetUserByRefreshToken(tokenBody); 
        
        
        var lists = await _listRepository.GetAllListsByUserId(neededUser.Id);

        return lists;


    }

    public  async Task<ServiceResult> DeleteListById(Guid id)
    {
        try
        {
            await _listRepository.DeleteEntityById(id);
            return new ListDeletedResult();
        }
        catch (Exception e)
        {
            return new EntityNotFoundResult();
        }
    }

    public async  Task<List<List>> GetAllLists()
    {
        return await _listRepository.GetAllLists();
    }
}