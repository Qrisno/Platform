using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Platform.API.Controllers;
using Platform.Application.DTOs;
using Platform.Application.Enums;
using Platform.Application.Models;
using Platform.Application.Services;

namespace Platform.API.Tests;

[Trait("Category", "Auth Controller Tests")]
public class AuthControllerTests
{
    private Mock<IAuthService> _authMock;
    private AuthController _authController;
    
    //heheheheheheeee sheeep moooocks
    public AuthControllerTests()
    {
        _authMock = new Mock<IAuthService>();
        _authController = new AuthController(_authMock.Object);
        
    }
    [Fact]
    public async Task Login_GivenNonExistingCredentials_ReturnsUserNotFoundStatusCode()
    {
        _authMock.Setup(service => service.Login(It.IsAny<LoginDTO>()))
            .ReturnsAsync(new LoginResponse { AuthStatus = AuthStatusEnum.NotFound, ReasonText = "User not found" }
            );
        var nonExistingUserLoginCredentials = new LoginDTO()
        {
            Email = "n@gm",
            Password = "11"
        };
        var res = await _authController.Login(nonExistingUserLoginCredentials);
        _authMock.Verify(service=>service.Login(It.IsAny<LoginDTO>()), Times.Once());
        var objectResult = Assert.IsType<ObjectResult>(res);

        Assert.Equal( StatusCodes.Status404NotFound
            , objectResult.StatusCode );
    }
    
    [Fact]
    public async Task Login_GivenExistingCredentials_ReturnsUserSuccessWithToken()
    {
        _authMock.Setup(service => service.Login(It.IsAny<LoginDTO>()))
            .ReturnsAsync(new LoginResponse { AuthStatus = AuthStatusEnum.Success, Token = "sampleToken" });
        var nonExistingUserLoginCredentials = new LoginDTO()
        {
            Email = "nini@gmail.com",
            Password = "Cxvrebi100"
        };
        var res = await _authController.Login(nonExistingUserLoginCredentials);
        _authMock.Verify(service=>service.Login(It.IsAny<LoginDTO>()), Times.Once());
        var objectResult = Assert.IsType<OkObjectResult>(res);

        Assert.Equal( StatusCodes.Status200OK, objectResult.StatusCode);
        var loginResponse = Assert.IsType<LoginResponse>(objectResult.Value);
        Assert.Equal("sampleToken", loginResponse.Token);
    }

    [Fact]
    public async Task Login_GivenInvalidCredentials_ReturnsInvalidCredentials()
    {
        _authMock.Setup(s=>s.Login(It.IsAny<LoginDTO>()))
            .ReturnsAsync(new LoginResponse { AuthStatus = AuthStatusEnum.InvalidCredentials, ReasonText = "Invalid credentials" });

        var res = await _authController.Login(new LoginDTO
        {
            Email = "nini@gmail.com",
            Password = "Cxvrebi100"
        });

       var objectResult =  Assert.IsType<ObjectResult>(res);

       
       _authMock.Verify(s=>s.Login(It.IsAny<LoginDTO>()), Times.Once());;
       

  
       Assert.Equal(StatusCodes.Status401Unauthorized, objectResult.StatusCode);
      
    }
    
    [Fact]
    public async Task Login_GivenSomeUnknownError_ReturnsInternalServerError()
    {
        _authMock.Setup(s=>s.Login(It.IsAny<LoginDTO>()))
            .ReturnsAsync(new LoginResponse {  });

        var res = await _authController.Login(new LoginDTO
        {
            Email = "nini@gmail.com",
            Password = "Cxvrebi100"
        });

        var objectResult =  Assert.IsType<StatusCodeResult>(res);

       
        _authMock.Verify(s=>s.Login(It.IsAny<LoginDTO>()), Times.Once());;
       

  
        Assert.Equal(StatusCodes.Status500InternalServerError, objectResult.StatusCode);
      
    }
}