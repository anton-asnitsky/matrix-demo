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
    public class TaskDataValidatorTests
    {
        [Fact]
        public Task Should_Pass_Validation_If_Task_Not_Exists() {
            var tasksStub = new List<UserTask>()
            {
            }.AsQueryable();

            var taskssMock = tasksStub.BuildMockDbSet<UserTask>();

            var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(databaseName: "TaskAPI")
                    .Options;

            var dataContextMock = new Mock<DataContext>(options);
            dataContextMock.Setup(x => x.UserTasks).Returns(taskssMock.Object);

            var dataValidator = new TaskDataValidator(dataContextMock.Object);
            var newTaskMock = new UserTask()
            {
                Name = "Test 1"
            };

            Func<Task> f = async () => await dataValidator.Validate(newTaskMock);

            f.Should().NotThrow<ConflictException>();

            return Task.CompletedTask;
        }

        [Fact]
        public Task Should_Throw_ConflictExceprion_If_Task_Exists()
        {
            var tasksStub = new List<UserTask>()
            {
                new UserTask() {  Name = "Test 1" }
            }.AsQueryable();

            var taskssMock = tasksStub.BuildMockDbSet<UserTask>();

            var options = new DbContextOptionsBuilder<DataContext>()
                    .UseInMemoryDatabase(databaseName: "TaskAPI")
                    .Options;

            var dataContextMock = new Mock<DataContext>(options);
            dataContextMock.Setup(x => x.UserTasks).Returns(taskssMock.Object);

            var dataValidator = new TaskDataValidator(dataContextMock.Object);
            var newTaskMock = new UserTask()
            {
                Name = "Test 1"
            };

            Func<Task> f = async () => await dataValidator.Validate(newTaskMock);

            f.Should().Throw<ConflictException>();

            return Task.CompletedTask;
        }
    }
}
