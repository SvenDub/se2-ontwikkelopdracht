using System;

namespace Inject
{
    public class TypeNotResolvedException : Exception
    {
        public Type Type { get; }

        public TypeNotResolvedException(Type t) : this(t, $"Could not resolve \"{t}\".")
        {
        }

        public TypeNotResolvedException(Type t, string message) : base(message)
        {
            Type = t;
        }

        public TypeNotResolvedException(Type t, Exception innerException)
            : this(t, $"Could not resolve \"{t}\".", innerException)
        {
        }

        public TypeNotResolvedException(Type t, string message, Exception innerException)
            : base(message, innerException)
        {
            Type = t;
        }
    }
}