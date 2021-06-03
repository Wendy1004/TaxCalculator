using System;
using System.Collections.Generic;
using System.Text;
using TaxCalculator.Dtos;
using TaxCalculator_Console.Interfaces;

namespace TaxCalculator_Console.Services
{
	public class DisplayService : IDisplayService
	{
		public DisplayService()
		{
		}

		public void Display(SalaryPackage salaryPackage)
		{
			Console.WriteLine("");
			Console.WriteLine("Calculating salary details...");
			Console.WriteLine("");
			Console.WriteLine("Gross package: " + this.formatToCurrency(salaryPackage.GrossPackageAnnual));
			Console.WriteLine("Superannuation: " + this.formatToCurrency(salaryPackage.SuperContributionAnnual));
			Console.WriteLine("");
			Console.WriteLine("Taxable income: " + this.formatToCurrency(salaryPackage.TaxableIncomeAnnual));
			Console.WriteLine("");
			Console.WriteLine("Deductions:");
			Console.WriteLine("Medicare Levy: " + this.formatToCurrency(salaryPackage.MedicalLevy));
			Console.WriteLine("Budget Repair Levy: " + this.formatToCurrency(salaryPackage.BudgetRepairLevy));
			Console.WriteLine("Income Tax: " + this.formatToCurrency(salaryPackage.IncomeTax));
			Console.WriteLine("");
			Console.WriteLine("Net income: " + this.formatToCurrency(salaryPackage.NetIncome));
			Console.WriteLine("Pay packet: " + this.formatToCurrency(salaryPackage.PayPackage) + this.paymentMethod(salaryPackage.PaymentFrequency));
			Console.WriteLine("");
		}

		private string formatToCurrency(decimal value) => value.ToString("C");
		
		private string formatToCurrency(int value) => value.ToString("C");

		private string paymentMethod(PaymentFequency frequency)
		{
			switch (frequency)
			{
				case PaymentFequency.Weekly: return " per week";
				case PaymentFequency.Monthly: return " per month";
				case PaymentFequency.Fortnightly: return " per fornight";
				default: return "";
			}
		}
	}
}
