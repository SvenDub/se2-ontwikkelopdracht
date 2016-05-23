using System;

namespace Ontwikkelopdracht.Persistence
{
    public class EntityNotFoundException : EntityException
    {
        public EntityNotFoundException() : base("No entity found matching search criteria")
        {
        }

        public EntityNotFoundException(string message) : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}