using System;

namespace Demo.Contracts.Persistance
{
    public interface IPersistanceService
    {
        void Create(IPersistanceData entityToCreate);
        IPersistanceData Get(long persistanceId);
        IPersistanceData Update(IPersistanceData entityToUpdate);
        void Delete(long persistanceData);
    }
}
