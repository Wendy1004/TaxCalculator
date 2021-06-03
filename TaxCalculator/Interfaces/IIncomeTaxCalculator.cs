using TaxCalculator.Dtos;

namespace TaxCalculator.Interfaces
{
	public interface IIncomeTaxCalculator
	{
		SalaryPackage GetSalaryPackage(decimal grossPackage, string payFrequency);
	}
}
