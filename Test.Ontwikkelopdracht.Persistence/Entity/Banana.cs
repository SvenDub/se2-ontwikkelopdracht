using Ontwikkelopdracht.Persistence;

namespace Test.Ontwikkelopdracht.Persistence.Entity
{
    [Entity]
    public class Banana
    {
        [Identity]
        public int SomeIdField { get; set; }

        [DataMember]
        public bool Bend { get; set; }
    }
}