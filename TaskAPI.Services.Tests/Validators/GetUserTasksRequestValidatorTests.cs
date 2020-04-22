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
    public class GetUserTasksRequestValidatorTests
    {
        [Fact]
        public void Should_Pass_On_Correct_Request()
        {
            var validator = new GetUserTasksRequestValidator();
            var mockRequest = new GetUserTasksRequest()
            {
                Email = "test@test.com"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(true);
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void Should_Fail_On_Empty_Email()
        {
            var validator = new GetUserTasksRequestValidator();
            var mockRequest = new GetUserTasksRequest()
            {
                Email = string.Empty
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Email cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Email_Roo_Long()
        {
            var validator = new GetUserTasksRequestValidator();
            var mockRequest = new GetUserTasksRequest()
            {
                Email = new string('&', 300)
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Email cannot be longer than 255 charcters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_PFail_On_Invalid_Email()
        {
            var validator = new GetUserTasksRequestValidator();
            var mockRequest = new GetUserTasksRequest()
            {
                Email = "test@testtest"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Valid email address shuld be provided.").Should().NotBeNull();
        }

    }
}
