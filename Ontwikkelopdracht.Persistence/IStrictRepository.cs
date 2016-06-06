namespace Ontwikkelopdracht.Persistence
{
    public interface IStrictRepository<T> : IRepository<T> where T : new()
    {
    }
}