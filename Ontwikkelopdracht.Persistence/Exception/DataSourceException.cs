namespace Ontwikkelopdracht.Persistence.Exception
{
    public class DataSourceException : System.Exception
    {
        private const string DefaultMessage = "Could not read from data source";

        public DataSourceException() : base(DefaultMessage)
        {
        }

        public DataSourceException(string message) : base(message)
        {
        }

        public DataSourceException(System.Exception innerException) : this(DefaultMessage, innerException)
        {
        }

        public DataSourceException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}