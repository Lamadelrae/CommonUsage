using System.Globalization;

namespace Utils.Currency
{
    public static class BrazilianCurrencyExtensions
    {
        public static string ToBrl(this decimal input, bool hasSymbol = false)
        {
            string output = input.ToString("c", CultureInfo.GetCultureInfo("pt-BR"));

            if (hasSymbol)
                output = output.Replace("R$", string.Empty).Trim();

            return output;
        }
    }
}
