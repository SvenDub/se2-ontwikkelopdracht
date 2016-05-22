using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "kaartje")]
    public class Ticket
    {
        [Identity(Column = "kaartje_id")]
        public int Id { get; set; }

        [DataMember(Column = "voorstelling_id", Type = DataType.Entity)]
        public Show Show { get; set; }

        [DataMember(Column = "stoel_id")]
        public int Seat { get; set; }

        [DataMember(Column = "bestelling_id")]
        public int Order { get; set; }
    }
}