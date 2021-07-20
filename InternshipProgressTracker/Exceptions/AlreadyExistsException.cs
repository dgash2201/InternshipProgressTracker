using System;

namespace InternshipProgressTracker.Exceptions
{
    /// <summary>
    /// Exception that means that an entity already exists
    /// </summary>
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() : base() { }

        public AlreadyExistsException(string message) : base(message) { }
    }
}
