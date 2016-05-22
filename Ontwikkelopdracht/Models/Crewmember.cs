using System;

namespace Ontwikkelopdracht.Models
{
    public class Crewmember
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Birth { get; set; }
        public Gender Gender { get; set; }
        public string Bio { get; set; }
    }
}