using FCGames.API.Filters;
using FCGames.Application.Dto;
using FCGames.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FCGames.API.Constants.AppConstants;

namespace FCGames.API.Controllers;

[Route("users")]
[ApiController]
public class UserController(ILogger<UserController> logger, IUserApplicationService userApplicationService) : BaseController(logger)
{
    private readonly IUserApplicationService _userApplicationService = userApplicationService;

    /// <summary>
    /// Criar um novo usuário
    /// </summary>
    /// <param name="user">Objeto com as propriedades para criar um novo usuário</param>
    /// <returns>Um objeto do usuário criado</returns>
    [HttpPost]
    [SkipUserFilter]
    [Produces("application/json")]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    public async Task<object> Create([FromBody] GuestUser user)
    {
        var entity = await _userApplicationService.Add(user);
        return Ok(entity);
    }
}