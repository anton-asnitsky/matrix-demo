using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Exceptions
{
    public class NotFoundException : Exception
    {
        public string[] Errors { get; }

        public NotFoundException(string[] errors)
        {
            Errors = errors;
        }
    }
}
