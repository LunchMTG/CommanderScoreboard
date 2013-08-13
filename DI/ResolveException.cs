using System;

namespace DI
{
    public class ResolveException : Exception
    {
        public ResolveException(string message)
            : base(message)
        {

        }
    }
}
