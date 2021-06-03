using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Dtos
{
	public class TaxConfig
	{
		public decimal SuperContributionPercent { get; set; }
		public TaxConfigItem[] MedicareLevyConfig { get; set; }
		public TaxConfigItem[] BudgeRepairLevyConfig { get; set; }
		public TaxConfigItem[] IncomeTaxConfig { get; set; }
	}
}
