using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternshipProgressTracker.Exceptions
{
    /// <summary>
    /// Exception that means that a request has incorrect data
    /// </summary>
    public class BadRequestException : Exception
    {
        public BadRequestException() : base() { }

        public BadRequestException(string message) : base(message) { }
    }
}
