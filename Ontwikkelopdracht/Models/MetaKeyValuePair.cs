using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "META")]
    public class MetaKeyValuePair
    {
        [Identity(Column = "ID")]
        public int Id { get; set; }

        [DataMember(Column = "KEY")]
        public string Key { get; set; }

        [DataMember(Column = "VALUE")]
        public string Value { get; set; }
    }
}