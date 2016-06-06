using System;

namespace Ontwikkelopdracht.Persistence
{
    /// <summary>
    ///     Provides types to an injector.
    /// </summary>
    public interface IRepositoryProvider
    {
        /// <summary>
        ///     The type of the repository.
        /// </summary>
        Type GetDatabaseType<T>() where T : new();

        /// <summary>
        ///     The type that specifies the connection parameters.
        /// </summary>
        Type ConnectionParamsContract { get; }

        /// <summary>
        ///     The type that implements the connection parameters.
        /// </summary>
        Type ConnectionParamsImpl { get; }
    }
}