using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Interfaces;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    public class RequestValidator : IRequestValidator
    {
        private readonly Dictionary<Type, IValidator> _validators;

        public RequestValidator()
        {
            _validators = new Dictionary<Type, IValidator>
            {
                { typeof(LoginRequest), new LoginRequestValidator() },
                { typeof(PasswordChangeRequest), new PasswordChangeRequestValidator() },
                { typeof(RecoverPasswordRequest), new RecoverPasswordRequestValidator() },
                { typeof(CreateUserRequest), new CreateUserRequestValidator() },
                { typeof(UpdateUserRequest), new UpdateUserRequestValidator() },
                { typeof(CreateTaskRequest), new CreateTaskRequestValidator() },
                { typeof(UpdateTaskRequest), new UpdateTaskRequestValidator() },
                { typeof(GetUserTasksRequest), new GetUserTasksRequestValidator() },
                { typeof(AssignTaskRequest), new AssignTaskRequestValidator()}
            };
        }

        public ValidationResult Validate<T>(T dto)
        {
            return _validators[typeof(T)].Validate(dto);
        }
    }
}
