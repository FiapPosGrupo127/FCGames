using AutoMapper;
using FCGames.Application.Dto;
using FCGames.Application.Interfaces;
using FCGames.Domain.Interfaces;
using EN = FCGames.Domain.Entities;

namespace FCGames.Application.Services;

public class UserApplicationService(IUserService userService, IMapper mapper) : IUserApplicationService
{
    private readonly IUserService _userService = userService;
    private readonly IMapper _mapper = mapper;

    public async Task<User> Add(GuestUser model)
    {
        var user = _mapper.Map<EN.User>(model);

        user = await _userService.Add(user);

        return _mapper.Map<User>(user);
    }
}