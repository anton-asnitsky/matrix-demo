using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAPI.Services.Models.Inbound;
using TaskAPI.Services.Validators;
using Xunit;

namespace TaskAPI.Services.Tests.Validators
{
    public class CreateTaskRequestValidatorTests
    {
        [Fact]
        public void Should_Pass_Correct_Request() {
            var validator = new CreateTaskRequestValidator();
            var mockRequest = new CreateTaskRequest() {
                Name = new string('*', 8),
                Priority = 1,
                TargetDate = DateTime.Now.AddDays(1),
                AssignTo = new List<Guid>() { 
                    Guid.NewGuid()
                }
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(true);
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void Should_Fail_On_Empty_Name()
        {
            var validator = new CreateTaskRequestValidator();
            var mockRequest = new CreateTaskRequest()
            {
                Name = string.Empty,
                Priority = 1,
                TargetDate = DateTime.Now.AddDays(1),
                AssignTo = new List<Guid>() {
                    Guid.NewGuid()
                }
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Task name cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Name_Too_Long()
        {
            var validator = new CreateTaskRequestValidator();
            var mockRequest = new CreateTaskRequest()
            {
                Name = new string('*', 300),
                Priority = 1,
                TargetDate = DateTime.Now.AddDays(1),
                AssignTo = new List<Guid>() {
                    Guid.NewGuid()
                }
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Task name cannot be longer than 255 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_TargetDate_Overdue()
        {
            var validator = new CreateTaskRequestValidator();
            var mockRequest = new CreateTaskRequest()
            {
                Name = new string('*', 8),
                Priority = 1,
                TargetDate = DateTime.Now.AddDays(-1),
                AssignTo = new List<Guid>() {
                    Guid.NewGuid()
                }
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Task target date cannot be lesser than now.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Wrong_Priority()
        {
            var validator = new CreateTaskRequestValidator();
            var mockRequest = new CreateTaskRequest()
            {
                Name = new string('*', 8),
                Priority = 0,
                TargetDate = DateTime.Now.AddDays(1),
                AssignTo = new List<Guid>() {
                    Guid.NewGuid()
                }
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Task priority value should be greater than or equal to 1.").Should().NotBeNull();

            mockRequest.Priority = 4;


            result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Task priority value should be lesser than or equal to 3.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Assignment_List()
        {
            var validator = new CreateTaskRequestValidator();
            var mockRequest = new CreateTaskRequest()
            {
                Name = new string('*', 8),
                Priority = 1,
                TargetDate = DateTime.Now.AddDays(-1),
                AssignTo = new List<Guid>() {
                }
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Task pshould be assign to at least one user.").Should().NotBeNull();
        }
    }
}
