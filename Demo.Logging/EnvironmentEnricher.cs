#region Usings

using Microsoft.Extensions.Configuration;
using Serilog.Core;
using Serilog.Events;

#endregion

namespace Demo.Logging
{
	public class EnvironmentEnricher : ILogEventEnricher
	{
		private readonly IConfiguration _configuration;

		public EnvironmentEnricher(IConfiguration configuration)
		{
			this._configuration = configuration;
		}

		public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
		{
			logEvent.AddPropertyIfAbsent(
				propertyFactory.CreateProperty("Environment", this._configuration["Environment"]));
		}
	}
}
