using Ontwikkelopdracht.Persistence.MySql;

namespace Ontwikkelopdracht.Models
{
    public class ProductionMySqlConnectionParams : IMySqlConnectionParams
    {
        public string Host { get; set; } = "db";
        public uint Port { get; set; } = 3306;
        public string Database { get; set; } = "se2";
        public string Username { get; set; } = "fontys";
        public string Password { get; set; } = "fontys";
    }
}