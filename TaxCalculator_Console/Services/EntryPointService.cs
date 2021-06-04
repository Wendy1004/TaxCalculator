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

		public EntryPointService(IDisplayService displayService, IConfiguration config, ILogger<EntryPointService> log)
		{
			_displayService = displayService;
			_config = config;
			_log = log;			
		}
		
		/// <summary>
		/// Main function for running the App Console
		/// </summary>
		public void Run()
		{
			try
			{
				var taxConfig = this.loadTaxConfig();
				if (taxConfig == null)
				{
					Console.WriteLine("Load tax configuration failed. App exit!");
					return;
				}

				
				do {
					var repeatFlag = true;
					while (repeatFlag)
					{
						repeatFlag = this.process(taxConfig);
					}
					if (repeatFlag == false)
					{
						Console.WriteLine("Do you want to continue (Y/N)? ");
					}
				} while (Console.ReadLine().ToUpper() == "Y");
			}
			catch (Exception ex)
			{
				_log.LogError(ex, "Application failed");
			}
		}

		/// <summary>
		/// process by getting inputs (SalaryPackage, paymentFrequency) and displaying results
		/// </summary>
		/// <param name="taxConfig"></param>
		private bool process(TaxConfig taxConfig)
		{
			var repeatFlag = false;
			Console.Clear();
			Console.WriteLine("Pay Calculator");

			// input Salary package
			Console.Write("Enter your salary package amount: ");
			string salaryInput = Console.ReadLine();
			if (!this.validateSalaryInput(salaryInput))
			{
				Console.ReadLine();
				repeatFlag = true;
				return repeatFlag;
			}

			// input pay frenquency
			Console.Write("Enter your pay frequency (W for weekly, F for fortnightly, M for monthly): ");
			string payFrequency = Console.ReadLine();
			if (!this.validatePayFrequencyInput(payFrequency))
			{
				Console.ReadLine();
				repeatFlag = true;
				return repeatFlag;
			}

			// all inputs ok
			SalaryPackage salaryPackage = new IncomeTaxCalculator(taxConfig).GetSalaryPackage(decimal.Parse(salaryInput), payFrequency);
			_displayService.Display(salaryPackage);

			return repeatFlag;
		}
				
		/// <summary>
		/// Validate the SalaryPackage input is correct or not 
		/// </summary>
		/// <param name="salarayInput"></param>
		/// <returns></returns>
		private bool validateSalaryInput(string salarayInput)
		{
			var isNumeric = decimal.TryParse(salarayInput, out decimal salary);
			if (!isNumeric || salary <= 0)
			{
				//check the input data 
				Console.WriteLine("Error: Wrong input on salary package! Press any key to re-enter the values");
				return false;
			}
			return true;
		}

		/// <summary>
		/// Validate the PaymentFrequency is correct form (W, F, M) or not
		/// </summary>
		/// <param name="payFrenquency"></param>
		/// <returns></returns>
		private bool validatePayFrequencyInput(string payFrenquency)
		{
			if (IncomeTaxCalculator.GetPaymentFrequency(payFrenquency) == PaymentFequency.Unknown)
			{
				//check the input data 
				Console.WriteLine("Error: Wrong input on pay frequency! Press any key to re-enter the values");
				return false;
			}
			return true;
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
				throw ex;
			}
			return taxConfig;
		}
	}
}
