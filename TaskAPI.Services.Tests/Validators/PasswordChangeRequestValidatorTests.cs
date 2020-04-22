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
    public class PasswordChangeRequestValidatorTests
    {
        [Fact]
        public void Should_Pass_On_Correct_Request()
        {
            var validator = new PasswordChangeRequestValidator();
            var mockRequest = new PasswordChangeRequest()
            {
                Token = new string('*', 10),
                Password = "P@$$w0rD"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(true);
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void Should_Fail_On_Invalid_Password()
        {
            var validator = new PasswordChangeRequestValidator();
            var mockRequest = new PasswordChangeRequest()
            {
                Token = new string('*', 10),
                Password = "password"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password should include at least on lower-case letter, one upper-case letter, one digit and at least one of special characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Password_Too_Long()
        {
            var validator = new PasswordChangeRequestValidator();
            var mockRequest = new PasswordChangeRequest()
            {
                Token = new string('*', 10),
                Password = new string('&', 300)
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password cannot be longer then 64 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Password()
        {
            var validator = new PasswordChangeRequestValidator();
            var mockRequest = new PasswordChangeRequest()
            {
                Token = new string('*', 10),
                Password = string.Empty
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Password_Too_Short()
        {
            var validator = new PasswordChangeRequestValidator();
            var mockRequest = new PasswordChangeRequest()
            {
                Token = new string('*', 10),
                Password = "pass"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password cannot be shorter then 8 characters.").Should().NotBeNull();
        }
    }
}
