#region Usings

using System;

#endregion

namespace Demo.Contracts.Logging
{
	/// <summary>
	/// Logging Service.
	/// </summary>
	public interface ILoggingService
	{
		/// <summary>
		/// Disposes all loggers.
		/// </summary>
		void CloseAndFlush();

		/// <summary>
		/// Logs Information messages with the system-logger, used for special log events like "Server started." independent from technical or performance logging.
		/// </summary>
		/// <param name="message">message text for logging</param>
		void LogSystemInformation(string message);

		void LogTechnicalDebug(string message, IProcessMetadata metadata = null);

		void LogPerformanceDebug(string message, IProcessMetadata metadata = null);

		void LogTechnicalInformation(string message, IProcessMetadata metadata = null);

		void LogPerformanceInformation(string message, IProcessMetadata metadata = null);

		void LogTechnicalWarning(string message, IProcessMetadata metadata = null);

		void LogPerformanceWarning(string message, IProcessMetadata metadata = null);

		void LogTechnicalError(string message, IProcessMetadata metadata = null);

		void LogPerformanceError(string message, IProcessMetadata metadata = null);

		void LogTechnicalFatal(Exception exception, string message, IProcessMetadata metadata = null);

		void LogPerformanceFatal(Exception exception, string message, IProcessMetadata metadata = null);

		void ForContextTechnical<T>();

		void ForContextPerformance<T>();

		void ForContextTechnical(Type source);

		void ForContextPerformance(Type source);

		void ForContextTechnical(string propertyName, object value, bool destructureObjects = false);

		void ForContextPerformance(string propertyName, object value, bool destructureObjects = false);
	}
}
