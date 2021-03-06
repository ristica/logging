﻿#region Usings

using System;
using Demo.Contracts;
using Demo.Contracts.Logging;
using Newtonsoft.Json;

#endregion

namespace Demo.Logging
{
    /// <summary>
    /// Logging Service with TechnicalLogging and Performance Logging.
    /// </summary>
    public class LoggingService : ILoggingService
	{

        #region Fieldsd

        private readonly ITechnicalLogging _technicalLogging;
		private readonly IPerformanceLogging _performanceLogging;
		private readonly ISystemLogging _systemLogging;
		private Serilog.ILogger _technicalLogger;
		private Serilog.ILogger _performanceLogger;
		private Serilog.ILogger _systemLogger;

        #endregion

        #region C-Tor

        public LoggingService(ITechnicalLogging technicalLogging, IPerformanceLogging performanceLogging, ISystemLogging systemLogging)
		{
            this._technicalLogging = technicalLogging;
            this._performanceLogging = performanceLogging;
            this._systemLogging = systemLogging;
            this._technicalLogger = technicalLogging.Logger ?? throw new ArgumentNullException(nameof(technicalLogging.Logger));
            this._performanceLogger = performanceLogging.Logger ?? throw new ArgumentNullException(nameof(performanceLogging.Logger));
            this._systemLogger = systemLogging.Logger ?? throw new ArgumentNullException(nameof(systemLogging.Logger));
		}

        #endregion

        public void CloseAndFlush()
		{
            this._technicalLogging.CloseAndFlush();
            this._performanceLogging.CloseAndFlush();
            this._systemLogging.CloseAndFlush();
		}

		public void LogSystemInformation(string message) => _systemLogger.Information(message);

		public void LogTechnicalDebug(string message, IProcessMetadata metadata = null) =>
			_technicalLogger.Debug(GetMessageWithMetaData(metadata, message));

		public void LogPerformanceDebug(string message, IProcessMetadata metadata = null) =>
			_performanceLogger.Debug(GetMessageWithMetaData(metadata, message));

		public void LogTechnicalInformation(string message, IProcessMetadata metadata = null) =>
			_technicalLogger.Information(GetMessageWithMetaData(metadata, message));

		public void LogPerformanceInformation(string message, IProcessMetadata metadata = null) =>
			_performanceLogger.Information(GetMessageWithMetaData(metadata, message));

		public void LogTechnicalWarning(string message, IProcessMetadata metadata = null) =>
			_technicalLogger.Warning(GetMessageWithMetaData(metadata, message));

		public void LogPerformanceWarning(string message, IProcessMetadata metadata = null) =>
			_performanceLogger.Warning(GetMessageWithMetaData(metadata, message));

		public void LogTechnicalError(string message, IProcessMetadata metadata = null) =>
			_technicalLogger.Error(GetMessageWithMetaData(metadata, message));

		public void LogPerformanceError(string message, IProcessMetadata metadata = null) =>
			_performanceLogger.Error(GetMessageWithMetaData(metadata, message));

		public void LogTechnicalFatal(Exception exception, string message, IProcessMetadata metadata = null) =>
			_technicalLogger.Fatal(exception, GetMessageWithMetaData(metadata, message));

		public void LogPerformanceFatal(Exception exception, string message, IProcessMetadata metadata = null) =>
			_performanceLogger.Fatal(exception, GetMessageWithMetaData(metadata, message));

		public void ForContextTechnical<T>() => _technicalLogger = _technicalLogger.ForContext<T>();

		public void ForContextPerformance<T>() => _performanceLogger = _performanceLogger.ForContext<T>();

		public void ForContextTechnical(Type source) => _technicalLogger = _technicalLogger.ForContext(source);

		public void ForContextPerformance(Type source) => _performanceLogger = _performanceLogger.ForContext(source);

		public void ForContextTechnical(string propertyName, object value, bool destructureObjects = false) =>
			_technicalLogger = _technicalLogger.ForContext(propertyName, value, destructureObjects);

		public void ForContextPerformance(string propertyName, object value, bool destructureObjects = false) =>
			_performanceLogger = _performanceLogger.ForContext(propertyName, value, destructureObjects);

		private static string GetMessageWithMetaData(IProcessMetadata metadata, string message)
		{
			if (metadata == null)
				return message;
			var messageObjToLog = new
			{
				message,
                metadata.SomeProperty
			};
			return JsonConvert.SerializeObject(messageObjToLog);
		}
    }
}
