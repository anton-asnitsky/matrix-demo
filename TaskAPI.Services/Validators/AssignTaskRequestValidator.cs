using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    public class AssignTaskRequestValidator : AbstractValidator<AssignTaskRequest>
    {
        public AssignTaskRequestValidator()
        {
            RuleFor(x => x.UsersToAssign.Count).GreaterThan(0)
            .WithMessage("Request shuld have at least one user to assign task to.");
        }
    }
}
