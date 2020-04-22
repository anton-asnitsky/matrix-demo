using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
    {
        public UpdateUserRequestValidator()
        {
            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("First name cannot be empty.");
            RuleFor(x => x.FirstName).MaximumLength(255)
                .WithMessage("First name cannot be longer than 255 characters.");

            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("Last name cannot be empty.");
            RuleFor(x => x.LastName).MaximumLength(255)
                .WithMessage("Last name cannot be longer than 255 characters.");

            RuleFor(x => x.Phone).NotEmpty()
                .WithMessage("Phone cannot be empty.");
            RuleFor(x => x.Phone).MaximumLength(20)
                .WithMessage("Phone be longer than 20 characters.");

            RuleFor(x => x.Address).NotEmpty()
                .WithMessage("Address cannot be empty.");

            RuleFor(x => x.Sex).GreaterThanOrEqualTo(1)
                .WithMessage("Sex should be greater than or equal 1.");
            RuleFor(x => x.Sex).LessThanOrEqualTo(2)
                .WithMessage("Sex should be lesser than or equal 2.");
        }
    }
}
