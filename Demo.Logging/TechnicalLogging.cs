#region Usings

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Demo.Contracts.Logging;

#endregion

namespace Demo.Logging
{
	public class TechnicalLogging : ITechnicalLogging
	{
		private const string _connectionString = "TechnicalLogging:ConnectionString";
		private const string _technicalLoggingLevelKey = "TechnicalLogging:MinimumLevel";
		private const string _technicalLoggingOverrideLevelKey = "TechnicalLogging:OverrideMicrosoftSystemLevel";
		private readonly ILoggerFactory _loggerFactory;
		private readonly ILoggingEnricher _loggingEnricher;
		private readonly IConfiguration _configuration;
		private Logger _loggerInstance;

		public TechnicalLogging(IConfiguration configuration, ILoggerFactory loggerFactory, ILoggingEnricher loggingEnricher)
		{
			_configuration = configuration;
			_loggerFactory = loggerFactory;
			_loggingEnricher = loggingEnricher;
		}

		public Serilog.ILogger Logger => _loggerInstance ?? CreateLogger();

		protected IConfiguration Configuration => _configuration;

		public Serilog.ILogger CreateLogger()
		{
			_loggerFactory.AddSerilog();
			var levelSwitch = ILSLoggingLevelSwitch();
			var overrideLevelSwitch = ILSLoggingOverrideLevelSwitch();
			var config = new LoggerConfiguration().ReadFrom.Configuration(_configuration)
				.Enrich.With(_loggingEnricher.CreateAllEnrichers());
			config.WriteTo.ILSLoggerSink(GetDatabaseConnectionString)
				.MinimumLevel.ControlledBy(levelSwitch)
				.MinimumLevel.Override("Microsoft", overrideLevelSwitch)
				.MinimumLevel.Override("System", overrideLevelSwitch);
			_loggerInstance = config.CreateLogger();
			// TODO : throw exception in case of Selflog?
			/* SelfLog.Enable(s =>
		   {
			   Console.WriteLine(s);
		   }); */
			return _loggerInstance;
		}

		public LoggingLevelSwitch ILSLoggingLevelSwitch()
		{
			var level = new LoggingLevelSwitch
			{
				MinimumLevel = ParseLogEventLevel(GetLogLevel())
			};
			return level;
		}

		public LoggingLevelSwitch ILSLoggingOverrideLevelSwitch()
		{
			var level = new LoggingLevelSwitch
			{
				MinimumLevel = ParseLogEventLevel(GetOverrideLogLevel())
			};
			return level;
		}

		public void CloseAndFlush()
		{
			_loggerInstance.Dispose();
		}

		protected virtual string GetDatabaseConnectionString()
		{
			return _configuration[_connectionString];
		}

		protected virtual string GetLogLevel()
		{
			return _configuration[_technicalLoggingLevelKey];
		}

		protected virtual string GetOverrideLogLevel()
		{
			return _configuration[_technicalLoggingOverrideLevelKey];
		}

		private LogEventLevel ParseLogEventLevel(string levelName)
		{
			LogEventLevel result;
			if (levelName == null)
				throw new ArgumentNullException(nameof(levelName));
			switch (levelName.ToUpperInvariant())
			{
				case "VERBOSE":
					result = LogEventLevel.Verbose;
					break;
				case "DEBUG":
					result = LogEventLevel.Debug;
					break;
				case "INFORMATION":
					result = LogEventLevel.Information;
					break;
				case "WARNING":
					result = LogEventLevel.Warning;
					break;
				case "ERROR":
					result = LogEventLevel.Error;
					break;
				case "FATAL":
					result = LogEventLevel.Fatal;
					break;
				default:
					throw new InvalidOperationException();
			}

			return result;
		}
	}
}
