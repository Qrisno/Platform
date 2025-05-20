using Microsoft.EntityFrameworkCore;
using Moq;
using Platform.Application.Enums;
using Platform.Application.Models;
using Platform.Application.Repos;
using Platform.Domain.Entities.Models;
using Platform.Infrastructure.Interfaces;
using Platform.Infrastructure.Repos;
using Platform.Tests.Common;

namespace Platform.Infrastructure.Tests
{
    [Trait("CourseRepo", "Course Repository CRUD Logic")]
    public class CoursesRepositoryTests
    {
        private readonly ICoursesRepository _coursesRepo;

        public CoursesRepositoryTests()
        {
            IQueryable<Course> courses = new List<Course>
            {
                new()
                {
                    CourseId = 101,
                    AuthorUserId = 1,
                    CourseTitle = "Introduction to Programming",
                    CourseDescription = "Basic programming concepts",
                    CourseLength = "6 weeks"
                },
                new()
                {
                    CourseId = 102,
                    AuthorUserId = 1,
                    CourseTitle = "Advanced Web Development",
                    CourseDescription = "Modern web development techniques",
                    CourseLength = "8 weeks"
                },
                new()
                {
                    CourseId = 103,
                    AuthorUserId = 2,
                    CourseTitle = "Machine Learning Basics",
                    CourseDescription = "Introduction to ML concepts",
                    CourseLength = "12 weeks"
                },
                new()
                {
                    CourseId = 104,
                    AuthorUserId = 3,
                    CourseTitle = "Mobile App Development",
                    CourseDescription = "Building iOS and Android apps",
                    CourseLength = "10 weeks"
                }
            }.AsQueryable();

            Mock<IPlatformDbContext> dbContext = new();
            Mock<DbSet<Course>> mockCoursesSet = new();
            mockCoursesSet.As<IQueryable<Course>>().Setup(m => m.Provider).Returns(courses.Provider);
            mockCoursesSet.As<IQueryable<Course>>().Setup(m => m.Expression).Returns(courses.Expression);
            mockCoursesSet.As<IQueryable<Course>>().Setup(m => m.ElementType).Returns(courses.ElementType);
            mockCoursesSet.As<IQueryable<Course>>().Setup(m => m.GetEnumerator())
                .Returns(() => courses.GetEnumerator());

            mockCoursesSet.As<IAsyncEnumerable<Course>>()
                .Setup(m => m.GetAsyncEnumerator(default))
                .Returns(new TestAsyncEnumerator<Course>(courses.GetEnumerator()));

            mockCoursesSet.As<IQueryable<Course>>()
                .Setup(m => m.Provider)
                .Returns(new TestAsyncQueryProvider<Course>(courses.Provider));

            mockCoursesSet.Setup(m => m.FindAsync(It.IsAny<object[]>()))
                .Returns((object[] ids) =>
                    ValueTask.FromResult(courses.FirstOrDefault(c => c.CourseId == (int)ids[0])));
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
            dbContext.Setup(db => db.Courses).Returns(mockCoursesSet.Object);
            dbContext.Setup(db => db.Users).Returns(mockSet.Object);
            _coursesRepo = new CoursesRepository(dbContext.Object);
        }

        [Fact]
        public async Task GetCoursesByAuthor_GivenExistingAuthorId_ReturnsCourses()
        {
            // Arrange
            int expectedAuthorId = 3;
            CourseResponse courseResponse = await _coursesRepo.GetCoursesByAuthor(expectedAuthorId);

            Assert.NotNull(courseResponse);
            Assert.IsType<CourseResponse>(courseResponse);
            Assert.NotEmpty(courseResponse.courses);
            Assert.IsType<Course>(courseResponse.courses[0]);
            Assert.Equal(expectedAuthorId, courseResponse.courses[0].AuthorUserId);
            Assert.Equal(CourseSearchResultEnum.Success, courseResponse.result);
        }

        [Fact]
        public async Task GetCoursesByAuthor_GivenNonExistingAuthorId_ReturnsEmptyCoursesResponse()
        {
            // Arrange
            int expectedAuthorId = 3456;
            CourseResponse courseResponse = await _coursesRepo.GetCoursesByAuthor(expectedAuthorId);

            Assert.NotNull(courseResponse);
            Assert.IsType<CourseResponse>(courseResponse);
            Assert.Empty(courseResponse.courses);
            Assert.Equal(CourseSearchResultEnum.NotFound, courseResponse.result);
        }
        
        [Fact]
        public async Task EnrollInCourse_GivenNonExistingCourse_ReturnsSuccess()
        {
            // Arrange
            CourseToEnrollDTO courseToEnroll = new()
            {
                CourseId = 999,
                UserId = 1
            };
            CourseResponse courseResponse = await _coursesRepo.EnrollInCourse(courseToEnroll);
            
            Assert.Equal(CourseSearchResultEnum.NotFound,courseResponse.result );
        }
        
        [Fact]
        public async Task EnrollInCourse_GivenNonExistingUserExistingCourse_ReturnsSuccess()
        {
            // Arrange
            CourseToEnrollDTO courseToEnroll = new()
            {
                CourseId = 101,
                UserId = 123123
            };
            CourseResponse courseResponse = await _coursesRepo.EnrollInCourse(courseToEnroll);
            
            Assert.Equal(CourseSearchResultEnum.UserNotFound,courseResponse.result );
        }

    }
}