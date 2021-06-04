using System;
using TaxCalculator.Dtos;
using TaxCalculator.Interfaces;

namespace TaxCalculator.Services
{
	public class IncomeTaxCalculator : IIncomeTaxCalculator
	{
		private decimal _grossPackage;
		private decimal _taxableIncome;
		private PaymentFequency _payFrequency;
		private TaxConfig _taxConfig;
		public IncomeTaxCalculator(TaxConfig taxConfig)
		{
			_taxConfig = taxConfig;
		}
		public SalaryPackage GetSalaryPackage(decimal grossPackage, string payFrequency)
		{
			_grossPackage = grossPackage;
			_payFrequency = IncomeTaxCalculator.GetPaymentFrequency(payFrequency);
			_taxableIncome = Math.Round(_grossPackage / (1 + _taxConfig.SuperContributionPercent/100), 2);

			var incomeTax = new SalaryPackage();
			incomeTax.PaymentFrequency = _payFrequency;
			if (incomeTax.PaymentFrequency != PaymentFequency.Unknown)
			{
				incomeTax.GrossPackageAnnual = _grossPackage;
				incomeTax.TaxableIncomeAnnual = _taxableIncome;
				incomeTax.SuperContributionAnnual = this.getSuperContribution();
				incomeTax.MedicalLevy = this.getMedicalLevy();
				incomeTax.BudgetRepairLevy = this.getBudgetRepairLevy();
				incomeTax.IncomeTax = this.getTax();
				decimal deductions = incomeTax.MedicalLevy + incomeTax.BudgetRepairLevy + incomeTax.IncomeTax;
				incomeTax.NetIncome = this.getNetIncome(_grossPackage, incomeTax.SuperContributionAnnual, deductions);
				incomeTax.PayPackage = this.getPayPackage(incomeTax.NetIncome);
			}
			return incomeTax;
		}

		/// <summary>
		/// Get Payment Frequency
		/// </summary>
		/// <returns></returns>
		public static PaymentFequency GetPaymentFrequency(string payFrequency)
		{
			switch (payFrequency.ToUpper())
			{
				case "W": return PaymentFequency.Weekly;
				case "F": return PaymentFequency.Fortnightly;
				case "M": return PaymentFequency.Monthly;
				default: return PaymentFequency.Unknown;
			}
		}

		/// <summary>
		/// Calculate Super Contribution 
		/// </summary>
		/// <returns>decimal</returns>
		private decimal getSuperContribution()
		{
			return (_grossPackage - _taxableIncome);
		}

		/// <summary>
		/// Calcuate Medical Levy (always rounded up to the nearest dollar)	
		/// </summary>
		/// <returns>int</returns>
		private int getMedicalLevy()
		{
			decimal retValue = this.calculate(_taxConfig.MedicareLevyConfig);
			return (int)Math.Ceiling(retValue);
		}

		/// <summary>
		/// Calcuate Budget Repair Levy (always rounded up to the nearest dollar)
		/// </summary>
		/// <returns>int</returns>
		private int getBudgetRepairLevy()
		{
			decimal retValue = this.calculate(_taxConfig.BudgeRepairLevyConfig);
			return (int)Math.Ceiling(retValue);
		}

		/// <summary>
		/// Calcuate Income Tax (always rounded up to the nearest dollar) 
		/// <returns>int</returns>
		private int getTax()
		{
			decimal retValue = this.calculate(_taxConfig.IncomeTaxConfig);
			return (int)Math.Round(retValue);
		}

		/// <summary>
		/// Calcuate Net Income (always rounded up to the nearest dollar) 
		/// </summary>
		/// <param name="grossPackage"></param>
		/// <param name="super"></param>
		/// <param name="deductions"></param>
		/// <returns>decimal</returns>
		private decimal getNetIncome(decimal grossPackage, decimal super, decimal deductions)
		{
			return (grossPackage - super - deductions);
		}

		/// <summary>
		/// Calcuate Paypackage by Weekly, Fornightly or Monthly		
		/// </summary>
		/// <param name="netIncome"></param>
		/// <returns></returns>
		private decimal getPayPackage(decimal netIncome)
		{
			decimal retValue = 0;
			switch (_payFrequency)
			{
				case PaymentFequency.Weekly: 
					retValue = netIncome / 52;
					break;
				case PaymentFequency.Fortnightly:
					retValue = netIncome / 26;
					break;
				case PaymentFequency.Monthly:
					retValue = netIncome / 12;
					break;
				default: break;

			}
			return Math.Round(retValue,2);
		}
		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="taxConfigItems"></param>
		/// <returns>decimal</returns>
		private decimal calculate(TaxConfigItem[] taxConfigItems)
		{
			decimal retValue = 0;
			for (int i = 0; i < taxConfigItems.Length; i++)
			{
				TaxConfigItem configItem = taxConfigItems[i];

				//configItem.Max = -1 mean maximum decimal value
				if (_taxableIncome >= configItem.Min && (configItem.Max == -1 || _taxableIncome <= configItem.Max))
				{
					retValue = configItem.BaseTax + (_taxableIncome - configItem.ExcessOver) * configItem.Percent / 100;
					break;
				}
			}
			return retValue;
		}

	}
}
