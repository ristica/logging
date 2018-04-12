namespace Demo.Contracts.Logging
{
	public interface ITechnicalLogging
	{
		Serilog.ILogger Logger { get; }

		Serilog.ILogger CreateLogger();

		void CloseAndFlush();
	}
}
