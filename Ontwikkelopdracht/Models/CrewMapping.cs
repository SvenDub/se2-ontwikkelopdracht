using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "FILM_CREW_MAP")]
    public class CrewMapping
    {
        [Identity(Column = "ID")]
        public int Id { get; set; }

        [DataMember(Column = "FILM_ID", Type = DataType.Entity)]
        public Film Film { get; set; }

        [DataMember(Column = "CREW_ID", Type = DataType.Entity)]
        public Crewmember Crewmember { get; set; }

        [DataMember(Column = "TYPE_ID", Type = DataType.Entity)]
        public CrewType CrewType { get; set; }
    }
}