#region Usings

using Microsoft.Extensions.DependencyInjection;
using Demo.Contracts.Logging;

#endregion

namespace Demo.Logging
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddWebAppLogging(this IServiceCollection services)
		{
			services.AddSingleton<ILoggingEnricher, LoggingEnricher>();
			services.AddSingleton<ITechnicalLogging, TechnicalLogging>();
			services.AddSingleton<IPerformanceLogging, PerformanceLogging>();
			services.AddSingleton<ISystemLogging, SystemLogging>();
			services.AddSingleton<ILoggingService, LoggingService>();
			return services;
		}

		public static IServiceCollection AddConsoleAppLogging(this IServiceCollection services)
		{
			services.AddSingleton<ILoggingEnricher, ConsoleLoggingEnricher>();
			services.AddSingleton<ITechnicalLogging, TechnicalLogging>();
			services.AddSingleton<IPerformanceLogging, PerformanceLogging>();
			services.AddSingleton<ISystemLogging, SystemLogging>();
			services.AddSingleton<ILoggingService, LoggingService>();
			return services;
		}
	}
}
