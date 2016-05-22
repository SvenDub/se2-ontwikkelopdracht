namespace Ontwikkelopdracht.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Show Show { get; set; }
        public int Seat { get; set; }
    }
}