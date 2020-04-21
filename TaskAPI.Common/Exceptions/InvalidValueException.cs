using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TaskAPI.Common.Exceptions
{
    public class InvalidValueException : Exception
    {
        public string[] Errors { get; }

        public InvalidValueException(string error)
        {
            Errors = new[] { error };
        }

        public InvalidValueException(string[] errors)
        {
            Errors = errors;
        }

        public InvalidValueException(List<InvalidValueException> exceptions)
        {
            Errors = exceptions.SelectMany(e => e.Errors).ToArray();
        }
    }
}
