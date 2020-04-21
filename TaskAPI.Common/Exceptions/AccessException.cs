using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Exceptions
{
    public class AccessException : Exception
    {
        public AccessException(string message) : base(message)
        {
        }
    }
}
