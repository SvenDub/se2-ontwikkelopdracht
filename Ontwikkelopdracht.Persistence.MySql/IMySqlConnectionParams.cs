namespace Ontwikkelopdracht.Persistence.MySql
{
    public interface IMySqlConnectionParams
    {
        string Host { get; set; }
        uint Port { get; set; }
        string Database { get; set; }
        string Username { get; set; }
        string Password { get; set; }
    }
}