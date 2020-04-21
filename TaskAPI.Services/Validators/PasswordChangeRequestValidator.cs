using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    public class PasswordChangeRequestValidator : AbstractValidator<PasswordChangeRequest>
    {
        public PasswordChangeRequestValidator()
        {
            RuleFor(x => x.Password).NotEmpty()
                .WithMessage("Password cannot be empty.");
            RuleFor(x => x.Password).MinimumLength(8)
                .WithMessage("Password cannot be shorter then 8 characters.");
            RuleFor(x => x.Password).MaximumLength(64)
                .WithMessage("Password cannot be longer then 64 characters.");
            RuleFor(x => x.Password)
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[\W_])")
                .WithMessage("Password should include at least on lower-case letter, one upper-case letter, one digit and at least one of special characters.");
        }
    }
}
