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
    public class CreateUserRequestValidatorTests
    {
        [Fact]
        public void Should_Pass_On_Correct_Request() {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest() { 
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(true);
            result.Errors.Count.Should().Be(0);
        }

        [Fact]
        public void Should_Fail_On_Empty_First_Name() {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = string.Empty,
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "First name cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_First_Name_Too_Long()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 300),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password cannot be longer than 255 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Last_Name()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = string.Empty,
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Last name cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Last_Name_Too_Long()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 300),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Last cannot be longer than 255 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Email()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = string.Empty,
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Email cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Email_Too_Long()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = new string('a', 300),
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Email cannot be longer than 255 charcters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Invalid_Email()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "testtesttesttest",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Valid email address shuld be provided.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Passowrd()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = string.Empty
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Passowrd_Too_Short()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "pass"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password cannot be shorter than 8 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Passowrd_Too_Long()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = new string('&', 300)
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password cannot be longer than 64 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Invalid_Passowrd()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 1,
                Password = "password"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Password should include at least one capital letter, one digit and one non-alohanumeric character.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Address()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = string.Empty,
                Phone = new string('1', 8),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Address cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Empty_Phone()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = string.Empty,
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Phone cannot be empty.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Phone_Too_Long()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 50),
                Sex = 1,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Phone be longer than 20 characters.").Should().NotBeNull();
        }

        [Fact]
        public void Should_Fail_On_Incorrect_Sex()
        {
            var validator = new CreateUserRequestValidator();
            var mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 0,
                Password = "P@$$w9rD"
            };

            var result = validator.Validate(mockRequest);

            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Sex should be greater than or equal to 1.").Should().NotBeNull();

            mockRequest = new CreateUserRequest()
            {
                FirstName = new string('*', 8),
                LastName = new string('*', 8),
                Email = "test@test.com",
                Address = new string('*', 8),
                Phone = new string('1', 8),
                Sex = 3,
                Password = "P@$$w9rD"
            };


            result = validator.Validate(mockRequest);
            result.IsValid.Should().Be(false);
            result.Errors.FirstOrDefault(e => e.ErrorMessage == "Sex should be lesser than or equal to 2.").Should().NotBeNull();
        }
    }
}
