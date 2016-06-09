using Ontwikkelopdracht.Persistence;

namespace Ontwikkelopdracht.Models
{
    [Entity(Table = "CREW_TYPE")]
    public class CrewType
    {
        [Identity(Column = "TYPE_ID")]
        public int Id { get; set; }

        [DataMember(Column = "NAAM")]
        public string Name { get; set; }
    }
}