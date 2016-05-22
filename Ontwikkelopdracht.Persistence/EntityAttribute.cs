using System;

namespace Ontwikkelopdracht.Persistence
{
    [AttributeUsage(AttributeTargets.Class)]
    public class EntityAttribute : Attribute
    {
        /// <summary>
        ///     The name of the table in which the Entity is stored.
        /// </summary>
        public string Table { get; set; }
    }
}