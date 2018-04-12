namespace Demo.Contracts.Persistance
{
    public interface IPersistanceData
    {
        long PersistanceDataId { get; set; }
        object SomeData { get; set; }
    }
}
