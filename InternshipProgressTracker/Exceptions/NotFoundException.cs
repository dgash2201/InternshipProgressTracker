using System;

namespace InternshipProgressTracker.Exceptions
{
    /// <summary>
    /// Exception that means that an entity was not found
    /// </summary>
    public class NotFoundException : Exception
    {
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }
    }
}
