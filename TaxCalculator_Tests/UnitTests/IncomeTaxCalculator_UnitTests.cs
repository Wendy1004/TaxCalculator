using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using TaxCalculator.Dtos;
using TaxCalculator.Services;

namespace TaxCalculator_Tests.UnitTests
{
	public class TaxCalculator_UnitTests
	{
		TaxConfig _taxConfig;
		[SetUp]
		public void Setup()
		{
			_taxConfig = new TaxConfig()
			{
				SuperContributionPercent = 9.5m,
				MedicareLevyConfig = new TaxConfigItem[] {
					new TaxConfigItem() {
						Min = 0,
						Max = 21335,
						ExcessOver = 0,
						Percent = 0,
						BaseTax = 0
					},
					new TaxConfigItem() {
						Min = 21336,
						Max = 26668,
						ExcessOver = 21335,
						Percent = 10,
						BaseTax = 0
					},
					new TaxConfigItem() {
						Min = 26669,
						Max = -1,  //Negative value is configured to skip checking upper limit
						ExcessOver = 0,
						Percent = 2,
						BaseTax = 0
					},
				},
				BudgeRepairLevyConfig = new TaxConfigItem[]{
					new TaxConfigItem() {
						Min = 0,
						Max = 180000,
						ExcessOver = 0,
						Percent = 0,
						BaseTax = 0
					},
					new TaxConfigItem() {
						Min = 180001,
						Max = -1,
						ExcessOver = 18000,
						Percent = 2,
						BaseTax = 0
					},
				},
				IncomeTaxConfig = new TaxConfigItem[]{
					new TaxConfigItem() {
						Min = 0,
						Max = 18200,
						ExcessOver = 0,
						Percent = 0,
						BaseTax = 0
					},
					new TaxConfigItem() {
						Min = 18201,
						Max = 37000,
						ExcessOver = 18200,
						Percent = 19,
						BaseTax = 0
					},
					new TaxConfigItem() {
						Min = 37001,
						Max = 87000,
						ExcessOver = 37000,
						Percent = 32.5m,
						BaseTax = 3572
					},
					new TaxConfigItem() {
						Min = 87001,
						Max = 180000,
						ExcessOver = 87000,
						Percent = 37,
						BaseTax = 19822
					},
					new TaxConfigItem() {
						Min = 180001,
						Max = -1, //Negative value is configured to skip checking upper limit
						ExcessOver = 180000,
						Percent = 47,
						BaseTax = 54232
					},
				},

			};
		}

		[Test]
		public void If_income_65000_monthly_frequency_then_super_5639_27()
		{
			var incomeTaxCalculator = new IncomeTaxCalculator(_taxConfig);
			var salaryPackage = incomeTaxCalculator.GetSalaryPackage(65000, "M");

			Assert.AreEqual(65000, salaryPackage.GrossPackageAnnual);
			Assert.AreEqual(5639.27, salaryPackage.SuperContributionAnnual);
			Assert.AreEqual(59360.73, salaryPackage.TaxableIncomeAnnual);
			Assert.AreEqual(1188, salaryPackage.MedicalLevy);
			Assert.AreEqual(0, salaryPackage.BudgetRepairLevy);
			Assert.AreEqual(10839, salaryPackage.IncomeTax);
			Assert.AreEqual(47333.73, salaryPackage.NetIncome);
			Assert.AreEqual(3944.48, salaryPackage.PayPackage);
		}

		[Test]
		public void If_income_65000_fornightly_frequency_then_pay_1820_53()
		{
			var incomeTaxCalculator = new IncomeTaxCalculator(_taxConfig);
			var salaryPackage = incomeTaxCalculator.GetSalaryPackage(65000, "F");

			Assert.AreEqual(47333.73, salaryPackage.NetIncome);
			Assert.AreEqual(1820.53, salaryPackage.PayPackage);
		}

		[Test]
		public void If_income_65000_weekly_frequency_then_pay_910_26()
		{
			var incomeTaxCalculator = new IncomeTaxCalculator(_taxConfig);
			var salaryPackage = incomeTaxCalculator.GetSalaryPackage(65000, "W");

			Assert.AreEqual(47333.73, salaryPackage.NetIncome);
			Assert.AreEqual(910.26, salaryPackage.PayPackage);
		}

		[Test]
		public void If_income_180000_weekly_frequency_then_super_15616_44()
		{
			var incomeTaxCalculator = new IncomeTaxCalculator(_taxConfig);
			var salaryPackage = incomeTaxCalculator.GetSalaryPackage(180000, "W");

			Assert.AreEqual(15616.44, salaryPackage.SuperContributionAnnual);
			Assert.AreEqual(48454, salaryPackage.IncomeTax);
			Assert.AreEqual(3288, salaryPackage.MedicalLevy);
		}
	}
}
