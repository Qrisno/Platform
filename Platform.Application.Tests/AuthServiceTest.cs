using Moq;
using Platform.Application.DTOs;
using Platform.Application.Enums;
using Platform.Application.Models;
using Platform.Application.Repos;
using Platform.Application.Services;
using Platform.Domain.Entities.Models;

namespace Platform.Application.Tests
{
    [Trait("Category", "Authorization Service Logic")]
    public class AuthServiceTest
    {
        private readonly AuthService _authService;
        private readonly Mock<IAuthRepository> _repoMock;


        public AuthServiceTest()
        {
            _repoMock = new Mock<IAuthRepository>();

            _authService = new AuthService(_repoMock.Object);
        }

        // Am casebis arsebobas ra azri aqvs mainc vmockav yvelafers
        [Fact]
        public async Task Login_GivenCorrectCredentials_ReturnsToken()
        {
            LoginDTO loginData = new() { Email = "test@example.com", Password = "password123" };
            LoginResponse expectedLoginResponse = new() { Token = "sampleToken123" };

            _repoMock.Setup(repo => repo.LoginAsync(It.IsAny<LoginDTO>())).ReturnsAsync(expectedLoginResponse);
            LoginResponse loginResponse = await _authService.Login(loginData);
            Assert.Equal(expectedLoginResponse.Token, loginResponse.Token);
            _repoMock.Verify(repo => repo.LoginAsync(It.IsAny<LoginDTO>()), Times.Once());
        }

        [Fact]
        public async Task Login_GivenIncorrectCredentials_ReturnsNull()
        {
            LoginDTO loginData = new() { Email = "incorrect@example.com", Password = "password123" };
            LoginResponse expectedLoginResponse = new() { Token = null };

            _repoMock.Setup(repo => repo.LoginAsync(It.IsAny<LoginDTO>())).ReturnsAsync(expectedLoginResponse);
            LoginResponse result = await _authService.Login(loginData);
            _repoMock.Verify(repo=> repo.LoginAsync(It.IsAny<LoginDTO>()), Times.AtLeastOnce);
            
            Assert.Null(result.Token);
        }

        [Fact]
        public async Task Register_GivenValidUserData_ReturnsOk()
        {
            UserToRegisterDTO userData = new()
            {
                UserType = 1,
                FirstName = "Test",
                LastName = "User",
                Email = "test@example.com",
                Password = "password123"
            };
            RegistrationResponse expectedRegistrationResponse = new()
            {
                AuthStatus = AuthStatusEnum.Success,
                User = new User { UserType = 1, FirstName = "Test", LastName = "User", Email = "test@example.com" }
            };
            _repoMock.Setup(repo => repo.RegisterAsync(It.IsAny<UserToRegisterDTO>()))
                .ReturnsAsync(expectedRegistrationResponse);
            RegistrationResponse res = await _authService.Register(userData);

            Assert.Equal(expectedRegistrationResponse, res);
        }
    }
}