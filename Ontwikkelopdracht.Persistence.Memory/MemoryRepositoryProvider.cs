﻿using System;

namespace Ontwikkelopdracht.Persistence.Memory
{
    public class MemoryRepositoryProvider : IRepositoryProvider
    {
        public Type GetDatabaseType<T>() where T : new() => typeof(MemoryRepository<T>);

        public Type ConnectionParamsContract => null;
        public Type ConnectionParamsImpl => null;
        public Type Setup => typeof(MemorySetup);
    }
}