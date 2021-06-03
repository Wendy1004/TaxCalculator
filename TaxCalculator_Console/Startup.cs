using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using TaxCalculation.Interfaces;
using TaxCalculation.Services;
using TaxCalculator.Interfaces;
using TaxCalculator.Services;
using TaxCalculator_Console.Services;
using TaxCalculator_Console.Interfaces;

namespace TaxCalculator_Console
{
	public static class Startup
	{
		public static IServiceCollection ConfigurationServices()
		{
			var services = new ServiceCollection();

			var builder = new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
				.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
				.AddEnvironmentVariables();
			IConfiguration configuration = builder.Build();
			services.AddSingleton(configuration);

			//create Logger
			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.FromLogContext()
				.CreateLogger();

			services.AddLogging(builder =>
			{
				builder.AddSerilog();
			});

			services.AddTransient<IDisplayService, DisplayService>();
			services.AddTransient<IIncomeTaxCalculator, IncomeTaxCalculator>();
			services.AddTransient<IEntryPointService, EntryPointService>();

			return services;
		}
	}
}
