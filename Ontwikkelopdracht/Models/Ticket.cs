using System.ComponentModel;
using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "KAARTJE")]
    public class Ticket
    {
        [Identity(Column = "KAARTJE_ID")]
        public int Id { get; set; }

        [DataMember(Column = "VOORSTELLING_ID", Type = DataType.Entity)]
        public Show Show { get; set; }

        [DataMember(Column = "STOEL_ID")]
        [DisplayName("Stoel")]
        public int Seat { get; set; }

        [DataMember(Column = "BESTELLING_ID", RawType = typeof(Order))]
        public int Order { get; set; }
    }
}