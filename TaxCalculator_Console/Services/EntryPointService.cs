using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TaxCalculation.Interfaces;
using TaxCalculator.Dtos;
using TaxCalculator.Services;
using TaxCalculator_Console;
using TaxCalculator_Console.Interfaces;

//DI, Serilog, Settings
namespace TaxCalculation.Services
{
	public class EntryPointService : IEntryPointService
	{
		private readonly ILogger<EntryPointService> _log;
		private readonly IConfiguration _config;
		private readonly IDisplayService _displayService;
		private readonly TaxConfig _taxConfig;

		public EntryPointService()
		{
		}

		public EntryPointService(IDisplayService displayService, IConfiguration config, ILogger<EntryPointService> log)
		{
			_displayService = displayService;
			_config = config;
			_log = log;
			_taxConfig = this.loadTaxConfig();
		}

		public void Run()
		{
			try
			{
				if (_taxConfig == null)
				{
					Console.WriteLine("Load tax configuration failed. App exit!");
					return;
				}

				Console.Clear();
				Console.WriteLine("Pay Calculator");

				Console.Write("Enter your salary package amount: ");
				var isNumeric = decimal.TryParse(Console.ReadLine(), out decimal salary);
				if (!isNumeric || salary <= 0)
				{
					//check the input data 
					Console.WriteLine("Error: Wrong input on salary package! Press any key to re-enter the values");
					Console.ReadLine();
					this.Run();
				}

				Console.Write("Enter your pay frequency (W for weekly, F for fortnightly, M for monthly): ");
				string payFrequency = Console.ReadLine();
				SalaryPackage salaryPackage = new IncomeTaxCalculator(_taxConfig).GetSalaryPackage(salary, payFrequency);
				if (salaryPackage.PaymentFrequency == PaymentFequency.Unknown)
				{
					//check the input data 
					Console.WriteLine("Error: Wrong input on pay frequency! Press any key to re-enter the values");
					Console.ReadLine();
					this.Run();
				}
				else
				{
					_displayService.Display(salaryPackage);
					Console.Write("Do you want to continue (Y/N):");
					string isContinue = Console.ReadLine();
					if (isContinue.ToUpper() == "Y") this.Run();
					else return;
				}
			}
			catch (Exception ex)
			{
				_log.LogError(ex, "Application failed");
			}
		}

		/// <summary>
		/// Load TaxConfig from appsettings.json. 
		/// </summary>
		/// <returns></returns>
		private TaxConfig loadTaxConfig()
		{
			TaxConfig taxConfig = null;
			try
			{
				_log.LogInformation("Tax configuration loading...");
				var taxConfigSection = _config.GetSection("TaxConfig");
				if (taxConfigSection != null)
				{
					taxConfig = taxConfigSection.Get<TaxConfig>();
				}
				_log.LogInformation("Tax configuration loaded.");
			}
			catch (Exception ex)
			{
				_log.LogError(ex, "Load tax configuration failed");
			}
			return taxConfig;
		}
	}
}
