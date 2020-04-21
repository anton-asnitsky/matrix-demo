using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    public class RecoverPasswordRequestValidator : AbstractValidator<RecoverPasswordRequest>
    {

        public RecoverPasswordRequestValidator() {
            const string errorMessage = "Incorrect email.";

            RuleFor(x => x.Email).NotEmpty()
                    .WithMessage(errorMessage);
            RuleFor(x => x.Email).MaximumLength(64)
                    .WithMessage(errorMessage);
            RuleFor(x => x.Email).EmailAddress()
                    .WithMessage(errorMessage);
        }
        
    }
}
