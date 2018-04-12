#region Usings

using Demo.Contracts.Logging;
using System;
using System.Data.SqlClient;

#endregion

namespace Demo.Persistance.Logger
{
	/// <summary>
	/// Wrapper for SqlConnection to enable Logging:
	/// -) Open, TryExecute: Try/catch Exception for Logging
	/// -) Open, TryExecute: LogTechnicalDebug "starting.." and LogTechnicalInformation for "Finished"
	/// </summary>
	public class SqlConnectionWithExceptionLogger : IDisposable
	{
		private readonly ILoggingService _loggingService;
		private readonly SqlConnection _connection;

		public SqlConnectionWithExceptionLogger(ILoggingService loggingService, string connectionString)
		{
			_loggingService = loggingService;
			_connection = new SqlConnection(connectionString);
		}

		public SqlConnection Connection => _connection;

		public SqlCommand CreateCommand()
		{
			return _connection.CreateCommand();
		}

		public void Open()
		{
			try
			{
				_loggingService.LogTechnicalDebug($"SqlConnectionWithExceptionLogger: Opening {_connection.ConnectionString}...");
				_connection.Open();
				_loggingService.LogTechnicalInformation($"SqlConnectionWithExceptionLogger: Open {_connection.ConnectionString} finished.");
			}
			catch (Exception e)
			{
				_loggingService.LogTechnicalFatal(e, $"SqlConnectionWithExceptionLogger: OpenConnection {_connection.ConnectionString} Failed!");
				throw;
			}
		}

		public void TryExecute(Action action)
		{
			try
			{
				_loggingService.LogTechnicalDebug($"TryExecute: TryExecute {action.Method}...");
				action();
				_loggingService.LogTechnicalInformation($"TryExecute: TryExecute {action.Method} finished.");
			}
			catch (Exception e)
			{
				_loggingService.LogTechnicalFatal(e, $"SqlConnectionWithExceptionLogger: TryExecute {action.Method} Failed!");
				throw;
			}
		}

		public void Dispose()
		{
			_connection?.Dispose();
		}
	}
}
