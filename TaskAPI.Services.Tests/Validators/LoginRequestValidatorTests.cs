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
    public class LoginRequestValidatorTests
    {
        private string errorMessage = "Incorrect email or password.";

        [Fact]
        public void Should_Pass_On_Correct_Request()
        {
            var validator = new LoginRequestValidator();
            var mockRequest = new LoginRequest()
            {
                Email = "test@test.com",
                Password = "P@$$w0rD"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(true);
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void Should_Fail_On_Empty_Email()
        {
            var validator = new LoginRequestValidator();
            var mockRequest = new LoginRequest()
            {
                Email = string.Empty,
                Password = "P@$$w0rD"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_Email_Too_Long()
        {
            var validator = new LoginRequestValidator();
            var mockRequest = new LoginRequest()
            {
                Email = new string('&', 300),
                Password = "P@$$w0rD"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Invalid_Email()
        {
            var validator = new LoginRequestValidator();
            var mockRequest = new LoginRequest()
            {
                Email = "test@testtest",
                Password = "P@$$w0rD"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Password()
        {
            var validator = new LoginRequestValidator();
            var mockRequest = new LoginRequest()
            {
                Email = "test@test.com",
                Password = string.Empty
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Password_Too_Short()
        {
            var validator = new LoginRequestValidator();
            var mockRequest = new LoginRequest()
            {
                Email = "test@test.com",
                Password = "pass"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Password_Too_Long()
        {
            var validator = new LoginRequestValidator();
            var mockRequest = new LoginRequest()
            {
                Email = "test@test.com",
                Password = new string('*', 300)
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Invalid_Password()
        {
            var validator = new LoginRequestValidator();
            var mockRequest = new LoginRequest()
            {
                Email = "test@test.com",
                Password = "password"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == errorMessage).Should().NotBeNull();
        }
    }
}
