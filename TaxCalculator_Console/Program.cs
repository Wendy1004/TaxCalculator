
using Microsoft.Extensions.DependencyInjection;
using TaxCalculation.Interfaces;
using TaxCalculator_Console;

//DI, Serilog, Settings
namespace TaxCalculation
{
	class Program
	{
		static void Main(string[] args)
		{
			var services = Startup.ConfigurationServices();
			var serviceProvider = services.BuildServiceProvider();

			serviceProvider.GetService<IEntryPointService>().Run();
		}
	}
}
