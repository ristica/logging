#region Usings


#endregion

namespace Demo.Contracts
{
    public interface IProcessMetadata
	{
		string SomeProperty { get; }
		void UpdateState(string state);
	}
}
