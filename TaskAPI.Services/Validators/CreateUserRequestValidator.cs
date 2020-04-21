using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    class CreateUserRequestValidator : AbstractValidator<CreateUserRequest>
    {
        public CreateUserRequestValidator()
        {
            RuleFor(x => x.Email).NotEmpty()
                    .WithMessage("Email cannot be empty.");
            RuleFor(x => x.Email).MaximumLength(255)
                .WithMessage("Email cannot be longer than 255 charcters.");
            RuleFor(x => x.Email).EmailAddress()
                .WithMessage("Valid email address shuld be provided.");

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Password cannot be empty.");
            RuleFor(x => x.Password).MinimumLength(8)
                .WithMessage("Password cannot be shorter than 8 characters.");
            RuleFor(x => x.Password).MaximumLength(64)
                .WithMessage("Password cannot be longer than 64 characters.");
            RuleFor(x => x.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[\W_])")
                .WithMessage("Password should include at least one capital letter, one digit and one non-alohanumeric character.");

            RuleFor(x => x.FirstName).NotEmpty()
                .WithMessage("First name cannot be empty.");
            RuleFor(x => x.FirstName).MaximumLength(255)
                .WithMessage("Password cannot be longer than 255 characters.");

            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("Last name cannot be empty.");
            RuleFor(x => x.LastName).MaximumLength(255)
                .WithMessage("Last cannot be longer than 255 characters.");

            RuleFor(x => x.Phone).NotEmpty()
                .WithMessage("Phone cannot be empty.");
            RuleFor(x => x.LastName).MaximumLength(20)
                .WithMessage("Phone be longer than 20 characters.");

            RuleFor(x => x.LastName).NotEmpty()
                .WithMessage("Last name cannot be empty.");
            RuleFor(x => x.LastName).MaximumLength(255)
                .WithMessage("Last cannot be longer than 255 characters.");

            RuleFor(x => x.Address).NotEmpty()
                .WithMessage("Address cannot be empty.");

            RuleFor(x => x.Sex).NotEmpty()
                .WithMessage("Sex cannot be empty.");
            RuleFor(x => x.Sex).GreaterThanOrEqualTo(1).LessThanOrEqualTo(2)
                .WithMessage("Sex should be set to 1 or 2.");
        }
    }
}
