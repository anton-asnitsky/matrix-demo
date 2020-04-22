using FluentAssertions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskAPI.Services.Models.Inbound;
using TaskAPI.Services.Validators;
using Xunit;

namespace TaskAPI.Services.Tests.Validators
{
    public class UpdateUserRequestValidatorTests
    {
        [Fact]
        public void Should_Pass_On_Correct_Request()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Sex = 1,
                Address = new string('*', 8),
                Phone = new string('*', 8),
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(true);
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void Should_Fail_On_Empty_First_Name()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = string.Empty,
                LastName = new string('*', 8),
                Sex = 1,
                Address = new string('*', 8),
                Phone = new string('*', 8),
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "First name cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_First_Name_Too_Long()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = new string('*', 300),
                LastName = new string('*', 8),
                Sex = 1,
                Address = new string('*', 8),
                Phone = new string('*', 8),
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "First name cannot be longer than 255 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Last_Name()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = string.Empty,
                Sex = 1,
                Address = new string('*', 8),
                Phone = new string('*', 8),
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Last name cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Last_Name_Too_Long()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 300),
                Sex = 1,
                Address = new string('*', 8),
                Phone = new string('*', 8),
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Last name cannot be longer than 255 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Phone()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Sex = 1,
                Address = new string('*', 8),
                Phone = string.Empty,
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Phone cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Phone_Too_Long()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Sex = 1,
                Address = new string('*', 8),
                Phone = new string('*', 300),
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Phone be longer than 20 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Address()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Sex = 1,
                Address = string.Empty,
                Phone = new string('*', 8),
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Address cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Invalid_Sex()
        {
            var validator = new UpdateUserRequestValidator();
            var mockRequest = new UpdateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Sex = 0,
                Address = new string('*', 8),
                Phone = new string('*', 8),
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Sex should be greater than or equal 1.").Should().NotBeNull();

            mockRequest.Sex = 3;

            result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Sex should be lesser than or equal 2.").Should().NotBeNull();
        }

    }
}
