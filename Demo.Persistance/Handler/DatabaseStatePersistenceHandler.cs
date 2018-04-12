#region Usings

using Demo.Contracts.Persistance;
using Demo.Persistance.Data;
using System;

#endregion

namespace Demo.Persistence.Handler
{
	/// <summary>
	/// Persistance Handler for Database.
	/// </summary>
	public class DatabaseStatePersistenceHandler : IPersistanceHandler
	{
		private readonly IPersistanceService _persistanceService;

		public DatabaseStatePersistenceHandler(IPersistanceService persistanceService)
		{
			this._persistanceService = persistanceService;
		}

		public IPersistanceData LoadOrCreate(long persistanceId, object someData)
		{
			var data = this._persistanceService.Get(persistanceId);
			if (data == null)
			{
				data = new PersistanceData
				{
					PersistanceDataId = persistanceId,
					SomeData = someData
				};

				this._persistanceService.Create(data);
			}

			return data;
		}

		public void Store(IPersistanceData data)
		{
            this._persistanceService.Update(data);
		}

        public IPersistanceData Load(long persistanceDataId)
        {
            return this._persistanceService.Get(persistanceDataId);
        }

        public IPersistanceData Load(long persistanceDataId, object data)
        {
            throw new NotImplementedException();
        }

        public void Remove(long persistanceDataId, object data)
        {
            this._persistanceService.Delete(persistanceDataId);
        }
    }
}
