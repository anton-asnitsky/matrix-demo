using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            const string errorMessage = "Incorrect email or password.";

            RuleFor(x => x.Email).NotEmpty()
                .WithMessage(errorMessage);
            RuleFor(x => x.Email).MaximumLength(64)
                .WithMessage(errorMessage);
            RuleFor(x => x.Email).EmailAddress()
                .WithMessage(errorMessage);

            RuleFor(x => x.Password).NotEmpty()
                .WithMessage(errorMessage);
            RuleFor(x => x.Password).MinimumLength(8)
                .WithMessage(errorMessage);
            RuleFor(x => x.Password).MaximumLength(64)
                .WithMessage(errorMessage);
            RuleFor(x => x.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[\W_])")
                .WithMessage(errorMessage);
        }
    }
}
