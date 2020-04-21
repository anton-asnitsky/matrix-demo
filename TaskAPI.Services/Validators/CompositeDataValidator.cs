using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskAPI.Common.Exceptions;
using TaskAPI.Services.Interfaces;

namespace TaskAPI.Services.Validators
{
    public class CompositeDataValidator : ICompositeDataValidator
    {
        private readonly IEnumerable<IDataValidator> _validators;
        public CompositeDataValidator(IEnumerable<IDataValidator> validators)
        {
            _validators = validators;
        }

        public async Task Validate(object dto)
        {
            var validators = _validators.Where(v => v.Handle(dto));

            var exceptions = new List<InvalidValueException>();

            foreach (var validator in validators)
            {
                try
                {
                    await validator.Validate(dto);
                }
                catch (InvalidValueException e)
                {
                    exceptions.Add(e);
                }
            }

            if (exceptions.Any())
            {
                throw new InvalidValueException(exceptions);
            }
        }

        public bool Handle(object dto)
        {
            throw new NotImplementedException();
        }
    }
}
