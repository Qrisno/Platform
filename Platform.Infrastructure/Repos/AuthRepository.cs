using Microsoft.EntityFrameworkCore;
using Platform.Application.DTOs;
using Platform.Application.Enums;
using Platform.Application.Models;
using Platform.Application.Repos;
using Platform.Domain.Entities.Models;
using Platform.Infrastructure.Helpers;
using System.Security.Cryptography;

namespace Platform.Infrastructure
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AuthHelper _authService;

        private readonly PlatformDbContext _dbContext;

        public AuthRepository(PlatformDbContext dbContext, AuthHelper authService)
        {
            _dbContext = dbContext;
            _authService = authService;
        }

        public async Task<LoginResponse> LoginAsync(LoginDTO user)
        {
            // aq exceptionebi rogor unda davhandlo ver mivxvdi
            Auth? userFound = await _dbContext.AuthUsers.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userFound == null)
            {
                return new LoginResponse { AuthStatus = AuthStatusEnum.NotFound, ReasonText = "User Not Found" };
            }

            byte[] passedPasswordHash = _authService.GetPasswordHash(user.Password, userFound.PasswordSalt);
            if (CryptographicOperations.FixedTimeEquals(passedPasswordHash, userFound.PasswordHash))
            {
                string token = _authService.GetToken(userFound.id);

                return new LoginResponse
                {
                    AuthStatus = AuthStatusEnum.Success, ReasonText = "User Authorized", Token = token
                };
            }

            return new LoginResponse
            {
                AuthStatus = AuthStatusEnum.InvalidCredentials, ReasonText = "Provided credentials are invalid"
            };
        }

        public async Task<RegistrationResponse> RegisterAsync(UserToRegisterDTO user)
        {
            Auth? userFound = await _dbContext.AuthUsers.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userFound != null)
            {
                return new RegistrationResponse
                {
                    AuthStatus = AuthStatusEnum.AlreadyExists,
                    ReasonText = "User With that Credentials Already Exsists"
                };
            }

            byte[] passSalt = _authService.GetPasswordSalt();
            byte[] passHash = _authService.GetPasswordHash(user.Password, passSalt);
            Auth authUser = new() { Email = user.Email, PasswordHash = passHash, PasswordSalt = passSalt };

            User userToRegister = new()
            {
                Email = user.Email, FirstName = user.FirstName, LastName = user.LastName, UserType = user.UserType
            };
            await _dbContext.AuthUsers.AddAsync(authUser);
            await _dbContext.Users.AddAsync(userToRegister);
            await _dbContext.SaveChangesAsync();
            User? AddedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == user.Email);
            return new RegistrationResponse
            {
                AuthStatus = AuthStatusEnum.Success, ReasonText = "User Registered Successfully", User = AddedUser
            };
        }
    }
}