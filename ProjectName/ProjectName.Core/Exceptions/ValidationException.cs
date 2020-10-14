using System;

namespace ProjectName.Core.Exceptions
{
    public class ValidationException : Exception
    {
        public ValidationException()
        {
            
        }

        public ValidationException(string message) : base(message)
        {
            
        }
    }
}