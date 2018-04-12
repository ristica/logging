#region Usings

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Demo.Contracts.Logging;

#endregion

namespace Demo.Logging
{
	public class PerformanceLogging : TechnicalLogging, IPerformanceLogging
	{
        #region Fields

        private const string _connectionString = "PerformanceLogging:ConnectionString";
		private const string _performanceLoggingLevelKey = "PerformanceLogging:MinimumLevel";
		private const string _performanceLoggingOverrideLevelKey = "PerformanceLogging:OverrideMicrosoftSystemLevel";

        #endregion

        #region C-Tor

        public PerformanceLogging(IConfiguration configuration, ILoggerFactory loggerFactory, ILoggingEnricher loggingEnricher)
			: base(configuration, loggerFactory, loggingEnricher)
		{
		}

        #endregion

        protected override string GetDatabaseConnectionString()
		{
			return Configuration[_connectionString];
		}

		protected override string GetLogLevel()
		{
			return Configuration[_performanceLoggingLevelKey];
		}

		protected override string GetOverrideLogLevel()
		{
			return Configuration[_performanceLoggingOverrideLevelKey];
		}
	}
}
