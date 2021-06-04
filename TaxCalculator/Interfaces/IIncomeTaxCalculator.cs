using TaxCalculator.Dtos;

namespace TaxCalculator.Interfaces
{
	public interface IIncomeTaxCalculator
	{
		public SalaryPackage GetSalaryPackage(decimal grossPackage, string payFrequency);			
	}
}
