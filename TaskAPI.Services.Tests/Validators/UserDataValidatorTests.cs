using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskAPI.Services.Validators;
using Xunit;
using Moq;
using System.Threading.Tasks;
using TaskAPI.Data.DataContexts;
using TaskAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using TaskAPI.Common.Exceptions;

namespace TaskAPI.Services.Tests.Validators
{
    public class UserDataValidatorTests
    {
        [Fact]
        public Task Should_Pass_Validation_If_User_Not_Exists()
        {
            var usersStub = new List<User>()
            {
            }.AsQueryable();

            var usersMock = usersStub.BuildMockDbSet<User>();

            var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(databaseName: "TaskAPI")
                    .Options;

            var dataContextMock = new Mock<DataContext>(options);
            dataContextMock.Setup(x => x.Users).Returns(usersMock.Object);

            var dataValidator = new UserDataValidator(dataContextMock.Object);
            var newUserMock = new User()
            {
                Email = "test@test.com"
            };

            Func<Task> f = async () => await dataValidator.Validate(newUserMock);

            f.Should().NotThrow<ConflictException>();

            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Throw_ConflictExceprion_If_User_Exists()
        {
            var usersStub = new List<User>()
            {
                new User() { Email = "test@test.com" }
            }.AsQueryable();

            var usersMock = usersStub.BuildMockDbSet<User>();

            var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(databaseName: "TaskAPI")
                    .Options;

            var dataContextMock = new Mock<DataContext>(options);
            dataContextMock.Setup(x => x.Users).Returns(usersMock.Object);

            var dataValidator = new UserDataValidator(dataContextMock.Object);
            var newUserMock = new User()
            {
                Email = "test@test.com"
            };

            Func<Task> f = async () => await dataValidator.Validate(newUserMock);

            f.Should().Throw<ConflictException>();

            return Task.CompletedTask;
        }
    }
}
