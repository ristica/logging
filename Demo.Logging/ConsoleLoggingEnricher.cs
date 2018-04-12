﻿#region Usings

using Demo.Contracts.Logging;
using Microsoft.Extensions.Configuration;
using Serilog.Core;

#endregion

namespace Demo.Logging
{
	public class ConsoleLoggingEnricher : ILoggingEnricher
	{
		private readonly IConfiguration _config;

		public ConsoleLoggingEnricher(IConfiguration config)
		{
			_config = config;
		}

		public ILogEventEnricher[] CreateAllEnrichers()
		{
			var enrichersList = new ILogEventEnricher[]
			{
				new ThreadIdEnricher(),
				new EnvironmentEnricher(_config)
			};
			return enrichersList;
		}
	}
}
