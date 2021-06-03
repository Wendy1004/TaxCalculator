using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Dtos
{
	public class SalaryPackage
	{
		public decimal GrossPackageAnnual { get; set; }
		public decimal SuperContributionAnnual { get; set; }
		public decimal TaxableIncomeAnnual { get;  set; }
		public int MedicalLevy { get; set; }
		public int BudgetRepairLevy { get; set; }
		public int IncomeTax { get; set; }
		public decimal NetIncome { get; set; }
		public decimal PayPackage { get; set; }
		public PaymentFequency PaymentFrequency { get; set; }

	}

	public enum PaymentFequency
	{
		Weekly,
		Fortnightly,
		Monthly,
		Unknown,
	}
}
