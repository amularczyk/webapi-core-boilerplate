using System;

namespace ProjectName.Core.Exceptions
{
    public class NoFoundException : Exception
    {
        public NoFoundException()
        {

        }

        public NoFoundException(string message) : base(message)
        {

        }
    }
}