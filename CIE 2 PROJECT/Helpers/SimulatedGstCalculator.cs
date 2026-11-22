using System;

namespace CIE_2_PROJECT.Helpers
{
    /// <summary>
    /// GST display math used by SkillLink's academic payment simulation.
    /// It does not calculate or represent tax owed on a real supply.
    /// </summary>
    public static class SimulatedGstCalculator
    {
        public const decimal DemoRate = 18m;

        public static decimal CalculateTax(decimal taxableAmount)
        {
            return CalculateTax(taxableAmount, DemoRate);
        }

        public static decimal CalculateTax(decimal taxableAmount, decimal rate)
        {
            if (taxableAmount < 0m)
                throw new ArgumentOutOfRangeException("taxableAmount");
            if (rate < 0m || rate > 100m)
                throw new ArgumentOutOfRangeException("rate");

            return decimal.Round(taxableAmount * rate / 100m, 2, MidpointRounding.AwayFromZero);
        }

        public static decimal CalculateClientTotal(decimal taxableAmount)
        {
            return taxableAmount + CalculateTax(taxableAmount);
        }
    }
}
