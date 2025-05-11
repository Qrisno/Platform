using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Application.Services;
using Platform.Domain.Entities.Models;

namespace Platform.API.Controllers;

[Authorize]
[ApiController]
[Route("api/Users")]
public class UserController : ControllerBase
{
    private readonly UserService _userService;
    public UserController(UserService userService)
    {
        _userService = userService;
    }
    [HttpGet("Get")]
    public async Task<IActionResult> GetUser([FromQuery] int id)
    {
        var userFound = await _userService.GetUser(id);

        if (userFound == null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        return Ok(userFound);
    }

    [HttpGet("Update")]
    public async Task<IActionResult> GetUser([FromBody] User user)
    {
        var userFound = await _userService.UpdateUser(user);

        if (userFound == null)
        {
            return StatusCode(StatusCodes.Status404NotFound);
        }
        return Ok(userFound);
    }
}