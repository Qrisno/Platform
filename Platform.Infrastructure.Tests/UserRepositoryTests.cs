using Microsoft.EntityFrameworkCore;
using Moq;
using Platform.Application.Repos;
using Platform.Domain.Entities.Models;
using Platform.Infrastructure.Interfaces;

namespace Platform.Infrastructure.Tests
{
    [Trait("UserRepo", "User Repository CRUD Logic")]
    public class UserRepositoryTests
    {
        private readonly IUserRepository _userRepository;

        public UserRepositoryTests()
        {
            Mock<IPlatformDbContext> dbContext = new();
            Mock<DbSet<User>> mockSet = new();

            List<User> users = new()
            {
                new User
                {
                    UserId = 1,
                    UserType = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Courses = [101, 102, 103]
                },
                new User
                {
                    UserId = 2,
                    UserType = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Courses = [201, 202]
                },
                new User
                {
                    UserId = 3,
                    UserType = 1,
                    FirstName = "Bob",
                    LastName = "Johnson",
                    Email = "bob.johnson@example.com",
                    Courses = [101, 103]
                }
            };

            IQueryable<User> queryableUsers = users.AsQueryable();

            // Setup the mock DbSet
            mockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(queryableUsers.Provider);
            mockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(queryableUsers.Expression);
            mockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(queryableUsers.ElementType);
            mockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => queryableUsers.GetEnumerator());

            // Setup Find method
            mockSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .Returns((object[] ids) => ValueTask.FromResult(
                    users.FirstOrDefault(u => u.UserId == (int)ids[0])));

            dbContext.Setup(db => db.Users).Returns(mockSet.Object);

            _userRepository = new UserRepository(dbContext.Object);
        }

        [Fact]
        public async Task GetUserByIdAsync_GivenExistingUserId_ReturnsUser()
        {
            // Arrange
            int expectedUserId = 1;

            // Act
            User user = await _userRepository.GetUserByIdAsync(expectedUserId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(expectedUserId, user.UserId);
            Assert.Equal("John", user.FirstName);
            Assert.Equal("Doe", user.LastName);
        }

        [Fact]
        public async Task GetUserByIdAsync_GivenNonExistingUserId_ReturnsNull()
        {
            // Arrange
            int nonExistingUserId = 999;

            // Act
            User? user = await _userRepository.GetUserByIdAsync(nonExistingUserId);

            // Assert
            Assert.Null(user);
        }

        [Fact]
        public async Task DeleteUserAsync_GivenExistingUserId_ReturnsTrue()
        {
            // Arrange
            int expectedUserId = 1;
            bool isUserDeleted = await _userRepository.DeleteUserAsync(expectedUserId);

            Assert.True(isUserDeleted);
        }

        [Fact]
        public async Task DeleteUserAsync_GivenNonExistingUserId_ReturnsFalse()
        {
            // Arrange
            int expectedUserId = 111;
            bool isUserDeleted = await _userRepository.DeleteUserAsync(expectedUserId);

            Assert.False(isUserDeleted);
        }

        [Fact]
        public async Task UpdateUserAsync_GivenExistingUser_ReturnsUserData()
        {
            User userToUpdate = new()
            {
                UserId = 1,
                FirstName = "Nini",
                LastName = "John",
                Courses = [200, 300, 101],
                Email = "n@mail.ru",
                UserType = 0
            };

            User? updatedUser = await _userRepository.UpdateUserAsync(userToUpdate);

            Assert.Equal(userToUpdate, updatedUser);
        }

        [Fact]
        public async Task UpdateUserAsync_GivenNonExistingUser_ReturnsNull()
        {
            User userToUpdate = new()
            {
                UserId = 111,
                FirstName = "Nini",
                LastName = "John",
                Courses = [200, 300, 101],
                Email = "n@mail.ru",
                UserType = 0
            };

            User? updatedUser = await _userRepository.UpdateUserAsync(userToUpdate);

            Assert.Null(updatedUser);
        }
    }
}