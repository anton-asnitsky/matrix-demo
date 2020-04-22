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
    public class AssignTaskRequestValidatorTests
    {
        [Fact]
        public void Should_Pass_Correct_Request() {
            var validator = new AssignTaskRequestValidator();
            var mockRequest = new AssignTaskRequest()
            {
                UsersToAssign = new List<Guid>() {
                    Guid.NewGuid()
                }
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(true);
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void Should_Fail_On_Empty_Users_List() {
            var validator = new AssignTaskRequestValidator();
            var mockRequest = new AssignTaskRequest()
            {
                UsersToAssign = new List<Guid>() {
                }
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().BeFalse();

            result.Errors.Count.Should().Be(1);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Request shuld have at least one user to assign task to.").Should().NotBeNull();
        }
    }
}
