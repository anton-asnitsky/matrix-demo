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
    public class RecoverPasswordRequestValidatorTests
    {
        private string errorMessage = "Incorrect email.";

        [Fact]
        public void Should_Pass_On_Correct_Request()
        {
            var validator = new RecoverPasswordRequestValidator();
            var mockRequest = new RecoverPasswordRequest()
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
            var validator = new RecoverPasswordRequestValidator();
            var mockRequest = new RecoverPasswordRequest()
            {
                Email = string.Empty,
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Email_Too_Long()
        {
            var validator = new RecoverPasswordRequestValidator();
            var mockRequest = new RecoverPasswordRequest()
            {
                Email = new string('a', 300),
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Invalid_Email()
        {
            var validator = new RecoverPasswordRequestValidator();
            var mockRequest = new RecoverPasswordRequest()
            {
                Email = "testtesttesttest",
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }
    }
}
