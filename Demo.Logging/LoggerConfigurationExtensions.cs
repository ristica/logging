#region Usings

using System;
using System.Collections.Generic;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

#endregion

namespace Demo.Logging
{
	public static class LoggerConfigurationExtensions
	{
		private const string _tableSchemaName = "dbo";
		private const string _tableName = "Logs";

		public static LoggerConfiguration ILSLoggerSink(this LoggerSinkConfiguration loggerConfiguration,
			Func<string> connectionstringFunc,
			LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
			int batchPostingLimit = MSSqlServerSink.DefaultBatchPostingLimit,
			TimeSpan? period = null,
			IFormatProvider formatProvider = null,
			bool autoCreateSqlTable = false,
			string schemaName = _tableSchemaName)
		{
			if (loggerConfiguration == null)
				throw new ArgumentNullException(nameof(loggerConfiguration));

			var defaultedPeriod = period ?? MSSqlServerSink.DefaultPeriod;

			var columnOptions = new ColumnOptions();
			columnOptions.Store.Remove(StandardColumn.MessageTemplate);
			columnOptions.Store.Remove(StandardColumn.LogEvent);
			columnOptions.Store.Remove(StandardColumn.Properties);
			columnOptions.Properties.ExcludeAdditionalProperties = true;
			columnOptions.Properties.OmitElementIfEmpty = true;
			columnOptions.Properties.UsePropertyKeyAsElementName = true;
			columnOptions.TimeStamp.ConvertToUtc = true;
			columnOptions.AdditionalDataColumns = new List<System.Data.DataColumn>
			{
				new System.Data.DataColumn { DataType = typeof(string), ColumnName = "ApplicationName" },
				new System.Data.DataColumn { DataType = typeof(string), ColumnName = "Environment" },
				new System.Data.DataColumn { DataType = typeof(string), ColumnName = "SourceContext" },
				new System.Data.DataColumn { DataType = typeof(int), ColumnName = "ThreadId" }
			};

			var connectionstring = connectionstringFunc();
			if (connectionstring == null)
				throw new ArgumentNullException(nameof(connectionstring));

			return loggerConfiguration.Sink(
				new MSSqlServerSink(
					connectionstring,
					_tableName,
					batchPostingLimit,
					defaultedPeriod,
					formatProvider,
					autoCreateSqlTable,
					columnOptions,
					schemaName), restrictedToMinimumLevel);
		}
	}
}
