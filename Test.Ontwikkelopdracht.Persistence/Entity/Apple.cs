using Ontwikkelopdracht.Persistence;

namespace Test.Ontwikkelopdracht.Persistence.Entity
{
    [Entity]
    public class Apple
    {
        [Identity]
        public int Id { get; set; }

        [DataMember(Type = DataType.Value)]
        public string Type { get; set; }
    }
}