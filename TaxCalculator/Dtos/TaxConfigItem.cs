using System;
using System.Collections.Generic;
using System.Text;

namespace TaxCalculator.Dtos
{
	public class TaxConfigItem
	{
		public decimal Min { get; set; }
		public decimal Max { get; set; }
		public decimal ExcessOver { get; set; }
		public decimal Percent { get; set; }
		/// <summary>
		/// Applicable only for Income Tax
		/// </summary>
		public decimal BaseTax { get; set; }
	}
}
