using System;

namespace Demo.Contracts.Persistance
{
    public interface IPersistanceHandler
    {
        IPersistanceData LoadOrCreate(long persistanceId, object someData);
        IPersistanceData Load(long persistanceDataId, object data);
        void Store(IPersistanceData data);
        void Remove(long persistanceDataId, object data = null);
    }
}
