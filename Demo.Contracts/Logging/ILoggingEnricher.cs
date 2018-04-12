#region Usings

using Serilog.Core;

#endregion

namespace Demo.Contracts.Logging
{
	public interface ILoggingEnricher
	{
		ILogEventEnricher[] CreateAllEnrichers();
	}
}
