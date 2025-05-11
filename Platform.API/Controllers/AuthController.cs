using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Platform.Application.DTOs;
using Platform.Application.Enums;
using Platform.Application.Models;
using Platform.Application.Services;

namespace Platform.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginData)
        {
            LoginResponse loginResult = await _authService.Login(loginData);
            if (loginResult.AuthStatus == AuthStatusEnum.NotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = loginResult.ReasonText });
            }

            if (loginResult.AuthStatus == AuthStatusEnum.Success)
            {
                return Ok(loginResult);

            }

            if (loginResult.AuthStatus == AuthStatusEnum.InvalidCredentials)
            {
                return StatusCode(StatusCodes.Status401Unauthorized, new { message = loginResult.ReasonText });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);

        }

        [HttpPost("regsiter")]
        public async Task<IActionResult> Register(UserToRegisterDTO user)
        {
            RegistrationResponse registrationResult = await _authService.Register(user);
            if (registrationResult.AuthStatus == AuthStatusEnum.NotFound)
            {
                return StatusCode(StatusCodes.Status404NotFound, new { message = registrationResult.ReasonText });
            }

            if (registrationResult.AuthStatus == AuthStatusEnum.Success)
            {
                return Ok(registrationResult);
            }

            if (registrationResult.AuthStatus == AuthStatusEnum.AlreadyExists)
            {
                return StatusCode(StatusCodes.Status409Conflict, new { message = registrationResult.ReasonText });
            }

            return StatusCode(StatusCodes.Status500InternalServerError);
        }
    }
}