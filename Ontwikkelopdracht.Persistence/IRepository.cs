using System.Collections.Generic;

namespace Ontwikkelopdracht.Persistence
{
    /// <summary>
    ///     Interface for generic CRUD operations on a repository for a specified type.
    /// </summary>
    /// <typeparam name="T">The entity type the repository manages.</typeparam>
    /// <typeparam name="ID">The type of the id of the entity the repository manages.</typeparam>
    public interface IRepository<T> where T : new()
    {
        /// <summary>
        ///     Returns the number of entities available
        /// </summary>
        /// <returns>The number of entities.</returns>
        long Count();

        /// <summary>
        ///     Deletes the entity with the given id.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        void Delete(int id);

        /// <summary>
        ///     Deletes the given entities.
        /// </summary>
        /// <param name="entities">The entities to delete.</param>
        void Delete(List<T> entities);

        /// <summary>
        ///     Deletes a given entity.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void Delete(T entity);

        /// <summary>
        ///     Deletes all entities managed by the repository.
        /// </summary>
        void DeleteAll();

        /// <summary>
        ///     Returns whether an entity with the given id exists.
        /// </summary>
        /// <param name="id">The entity to check.</param>
        /// <returns>True if an entity with the given id exists, false otherwise.</returns>
        bool Exists(int id);

        /// <summary>
        ///     Returns all instances of the type.
        /// </summary>
        /// <returns>All entities.</returns>
        List<T> FindAll();

        /// <summary>
        ///     Returns all instances of the type with the given IDs.
        /// </summary>
        /// <param name="ids">The IDs to check.</param>
        /// <returns>All entities with the given IDs.</returns>
        List<T> FindAll(List<int> ids);

        /// <summary>
        ///     Retrieves an entity by its id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The entity with the given id or null if none found.</returns>
        T FindOne(int id);

        /// <summary>
        ///     Saves a given entity.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        /// <returns>The saved entity.</returns>
        T Save(T entity);

        /// <summary>
        ///     Saves all given entities.
        /// </summary>
        /// <param name="entities">The entities to save.</param>
        /// <returns>The saved entities.</returns>
        List<T> Save(List<T> entities);
    }
}