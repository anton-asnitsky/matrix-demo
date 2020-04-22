using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    public class GetUserTasksRequestValidator : AbstractValidator<GetUserTasksRequest>
    {
        public GetUserTasksRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                    .WithMessage("Email cannot be empty.");
            RuleFor(x => x.Email).MaximumLength(255)
                .WithMessage("Email cannot be longer than 255 charcters.");
            RuleFor(x => x.Email).EmailAddress()
                .WithMessage("Valid email address shuld be provided.");
        }
    }
}
