using Demo.Contracts.Persistance;
using Demo.Persistance.Data;
using System.Collections.Concurrent;

namespace Demo.Persistance.Handler
{
    public class MemoryPersistanceHandler : IPersistanceHandler
    {
        private readonly ConcurrentDictionary<StateKey, IPersistanceData> _storage = new ConcurrentDictionary<StateKey, IPersistanceData>();

        public IPersistanceData Load(long persistanceDataId, object data)
        {
            var key = new StateKey
            {
                PersistanceDataId = persistanceDataId,
                Data = data
            };
            return _storage.TryGetValue(key, out var item) ? item : null;
        }

        public IPersistanceData LoadOrCreate(long persistanceDataId, object data)
        {
            var key = new StateKey
            {
                PersistanceDataId = persistanceDataId,
                Data = data
            };

            var newData = new PersistanceData
            {
                PersistanceDataId = persistanceDataId,
                SomeData = data
            };

            var item = this._storage.AddOrUpdate(key, newData, (k, oldData) =>
            {
                return oldData;
            });
            return item;
        }

        public void Store(IPersistanceData data)
        {
            var key = new StateKey
            {
                PersistanceDataId = data.PersistanceDataId,
                Data = data.SomeData
            };

            this._storage.AddOrUpdate(key, data, (k, oldData) =>
            {
                return data;
            });
        }

        public void Remove(long persistanceDataId, object data)
        {
            var key = new StateKey
            {
                PersistanceDataId = persistanceDataId,
                Data = data
            };
            this._storage.TryRemove(key, out var _);
        }

        private struct StateKey
        {
            public long PersistanceDataId { get; set; }
            public object Data { get; set; }
        }
    }
}
