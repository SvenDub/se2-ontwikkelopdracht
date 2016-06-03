namespace Ontwikkelopdracht.Persistence.Exception
{
    public class EntityException : System.Exception
    {
        public EntityException() : base("The type is not a valid entity")
        {
        }

        public EntityException(string message) : base(message)
        {
        }

        public EntityException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}