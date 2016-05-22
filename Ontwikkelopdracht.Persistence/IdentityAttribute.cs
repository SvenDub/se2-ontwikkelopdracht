using System;

namespace Ontwikkelopdracht.Persistence
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IdentityAttribute : Attribute
    {
        public string Column { get; set; }
    }
}