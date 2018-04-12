#region Usings

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Demo.Contracts.Logging;

#endregion

namespace Demo.Logging
{
	public class SystemLogging : TechnicalLogging, ISystemLogging
	{
		private const string _connectionString = "SystemLogging:ConnectionString";
		private const string _loggingLevelKey = "SystemLogging:MinimumLevel";
		private const string _loggingOverrideLevelKey = "SystemLogging:OverrideMicrosoftSystemLevel";

		public SystemLogging(IConfiguration configuration, ILoggerFactory loggerFactory, ILoggingEnricher loggingEnricher)
			: base(configuration, loggerFactory, loggingEnricher)
		{
		}

		protected override string GetDatabaseConnectionString()
		{
			return Configuration[_connectionString];
		}

		protected override string GetLogLevel()
		{
			return Configuration[_loggingLevelKey];
		}

		protected override string GetOverrideLogLevel()
		{
			return Configuration[_loggingOverrideLevelKey];
		}
	}
}
