using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Services.Interfaces
{
    public interface IRequestValidator
    {
        ValidationResult Validate<T>(T dto);
    }
}
