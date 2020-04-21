using System;
using System.Collections.Generic;
using System.Text;

namespace TaskAPI.Common.Exceptions
{
    public class NotModifiedException : Exception
    {
        public NotModifiedException(string message) : base(message)
        {

        }
    }
}
