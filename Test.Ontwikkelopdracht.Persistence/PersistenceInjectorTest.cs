using Inject;
using NUnit.Framework;
using Ontwikkelopdracht.Persistence;
using Ontwikkelopdracht.Persistence.Memory;
using Test.Ontwikkelopdracht.Persistence.Entity;

namespace Test.Ontwikkelopdracht.Persistence
{
    public class PersistenceInjectorTest
    {
        [OneTimeSetUp]
        public void SetUp() => PersistenceInjector.Inject(new MemoryRepositoryProvider());

        [OneTimeTearDown]
        public void TearDown() => Injector.Reset();

        [Test]
        public void TestInjection()
        {
            Assert.IsInstanceOf<RepositoryArmour<Apple>>(Injector.Resolve<IRepository<Apple>>());
            Assert.IsInstanceOf<RepositoryArmour<Banana>>(Injector.Resolve<IRepository<Banana>>());

            Assert.IsInstanceOf<MemoryRepository<Apple>>(Injector.Resolve<IStrictRepository<Apple>>());
            Assert.IsInstanceOf<MemoryRepository<Banana>>(Injector.Resolve<IStrictRepository<Banana>>());
        }
    }
}