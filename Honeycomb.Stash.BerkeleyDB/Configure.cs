namespace Honeycomb.Stash.BerkeleyDB
{
    using Events;
    using global::Stash;
    using global::Stash.BerkeleyDB;
    using global::Stash.BerkeleyDB.Engine;
    using Microsoft.Practices.ServiceLocation;

    public static class Configure
    {
        public static void Defaults(IServiceLocator serviceLocator, string storeDirectoryPath)
        {
            Kernel.Kickstart(
                new BerkeleyBackingStore(new DefaultBerkeleyBackingStoreEnvironment(storeDirectoryPath)),
                register =>
                {
                    register.Graph<StoredEvent>();
                    register.Index(new EventByReceivedTimestamp());
                });
        }
    }
}