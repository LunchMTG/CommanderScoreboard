using System;

namespace DI
{
    public class RegisterException : Exception
    {
        public RegisterException(string message) : base(message) { }
    }
}
