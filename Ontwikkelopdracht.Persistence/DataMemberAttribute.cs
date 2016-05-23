using System;

namespace Ontwikkelopdracht.Persistence
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DataMemberAttribute : Attribute
    {
        public string Column { get; set; }
        public DataType Type { get; set; } = DataType.Value;
        public Type RawType { get; set; } = typeof(int);
    }
}