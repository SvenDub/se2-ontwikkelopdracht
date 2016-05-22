using System;

namespace Ontwikkelopdracht.Persistence
{
    public class EntityException : Exception
    {
        public EntityException() : base("The type is not a valid entity")
        {
        }

        public EntityException(string message) : base(message)
        {
        }

        public EntityException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}