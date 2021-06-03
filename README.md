# Tax Calculator

This is a quick calculator that allows users to enter a gross salary package and a pay frequency, and then display a breakdown of this salary and what their pay package will be.

## Usage

- Enter your gross salary package. For example: 65000
- Then enter your pay frequency (W for weekly, F for fortnightly, M for monthly). For example: M

You will see your monthly packet as below:

```
Calculating salary details...

Gross package: $65,000.00
Superannuation: $5,639.27

Taxable income: $59,360.73

Deductions:
Medicare Levy: $1,188.00
Budget Repair Levy: $0.00
Income Tax: $10,839.00

Net income: $47,333.73
Pay packet: $3,944.48 per month

Do you want to continue (Y/N):  -> Enter Y to continue, N to quit the app.
```

## Configuration

The application allows user to configure tax rates in appsettings.json if the tax rate changes.

- SuperContributionPercent: 9.5
- MedicareLevyConfig is a table of Medicare Levy based on Income
- BudgeRepairLevyConfig is a table of Budget Repair Levy based on Income
- IncomeTaxConfig is a table ofIncome Tax based on Income

Note: The maximum gross package will be the maximum positive value of decimal, which is extremely over the practical value of gross salary package.

## Design and implementation

- The application is designed and implemented with .Net Core 3.1, Dependency Injection, Serilog for logging, and NUnit for Unit Tests.
- Business exceptions are processed and display nicely to users.
- Simple overall technical error handling will catch and log all exceptions into a daily rolling log file.

## Roadmap

- The application is designed and built to be testable, and easy to enhance.
- For example, the TaxCalculator project or DLL can be reused in other project easily such as in a WebApi project, or even in AWS lambda or Azure functions.
- Dockerize the project should be easy to.

- The taxable income can be configured and extended, which will help to address different tax rates on different year.
