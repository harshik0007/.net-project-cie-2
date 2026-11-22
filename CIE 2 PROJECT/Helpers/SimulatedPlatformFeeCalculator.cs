using System;

namespace CIE_2_PROJECT.Helpers
{
    /// <summary>
    /// Calculates the platform's academic-simulation fee on the pre-GST
    /// contract amount, for either side of a normally completed payment.
    /// </summary>
    public static class SimulatedPlatformFeeCalculator
    {
        public const decimal DemoRate = 5m;

        public static decimal CalculateFee(decimal contractAmount)
        {
            return CalculateFee(contractAmount, DemoRate);
        }

        public static decimal CalculateFee(decimal contractAmount, decimal rate)
        {
            if (contractAmount < 0m)
                throw new ArgumentOutOfRangeException("contractAmount");
            if (rate < 0m || rate > 100m)
                throw new ArgumentOutOfRangeException("rate");

            return decimal.Round(contractAmount * rate / 100m, 2, MidpointRounding.AwayFromZero);
        }
    }
}
