namespace Honeycomb.Stash.BerkeleyDB
{
    using System;
    using Events;
    using global::Stash;
    using global::Stash.BerkeleyDB;
    using global::Stash.BerkeleyDB.Engine;
    using global::Stash.Configuration;
    using Microsoft.Practices.ServiceLocation;

    public static class Configure
    {
        public static void Defaults(IServiceLocator serviceLocator, string storeDirectoryPath, Action<PersistenceContext<BerkeleyBackingStore>> stashRegistration)
        {
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(storeDirectoryPath)),
                register =>
                {
                    register.Graph<StoredEvent>();
                    register.Index(new EventsByReceivedTimestamp());
                    register.Index(new EventsByType());
                    stashRegistration(register);
                });
        }
    }
}