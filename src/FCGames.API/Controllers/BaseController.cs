using Microsoft.AspNetCore.Mvc;

namespace FCGames.API.Controllers;

[ApiController]
public class BaseController(ILogger<BaseController> logger) : ControllerBase
{
    private readonly ILogger<BaseController> _logger = logger;
}
