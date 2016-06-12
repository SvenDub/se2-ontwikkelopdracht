namespace Ontwikkelopdracht.Persistence.Memory
{
    public class MemorySetup : IRepositorySetup
    {
        /// <summary>
        ///     No setup is required for an in memory database, so this method does nothing.
        /// </summary>
        public bool Setup() => true;
    }
}