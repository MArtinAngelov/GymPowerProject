using System;

namespace GymPower.Helpers
{
    public static class CurrencyHelper
    {
        private const decimal ExchangeRate = 1.95583m; // Fixed peg BGN -> EUR

        public static string ToEuro(this decimal priceBgn)
        {
            var priceEur = priceBgn / ExchangeRate;
            return $"{priceEur:F2} €";
        }

        public static string ToBgn(this decimal priceBgn)
        {
            return $"{priceBgn:F2} лв.";
        }

        // Alias for generic usage
        public static string Format(decimal priceBgn) => ToBgn(priceBgn);
    }
}
