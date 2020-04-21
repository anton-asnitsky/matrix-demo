using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using TaskAPI.Services.Models.Inbound;

namespace TaskAPI.Services.Validators
{
    public class CreateTaskRequestValidator : AbstractValidator<CreateTaskRequest>
    {
        public CreateTaskRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .WithMessage("Task name cannot be empty.");
            RuleFor(x => x.Name).MaximumLength(255)
                .WithMessage("Task name cannot be longer than 255 characters.");

            RuleFor(x => x.TargetDate).NotEmpty()
                .WithMessage("Task target date cannot be empty.");
            RuleFor(x => x.TargetDate.ToUniversalTime()).GreaterThan(DateTime.UtcNow)
                .WithMessage("Task target date cannot be lesser than now.");

            RuleFor(x => x.Priority).NotEmpty()
                .WithMessage("Task priority cannot be empty.");
            RuleFor(x => x.Priority).GreaterThanOrEqualTo(1).LessThanOrEqualTo(3)
                .WithMessage("Task priority value should be set between 1 to 3.");

            RuleFor(x => x.AssignTo.Count).GreaterThan(0)
                .WithMessage("Task pshould be assign to at least one user.");
        }
    }
}
