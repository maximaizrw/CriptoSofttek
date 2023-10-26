namespace CriptoSofttek.Helpers
{
    public class ExchangeHelper
    {
        public static decimal ConvertPesosToUSD(decimal pesosAmount)
        {
            // Tasa de cambio: 1 USD = 730 pesos
            decimal usdExchangeRate = 730;

            // Realiza la conversión de pesos a dólares
            decimal usdAmount = pesosAmount / usdExchangeRate;

            return usdAmount;
        }

        public static decimal ConvertUSDToPesos(decimal usdAmount)
        {
            // Tasa de cambio: 1 USD = 730 pesos
            decimal pesosExchangeRate = 730;

            // Realiza la conversión de dólares a pesos
            decimal pesosAmount = usdAmount * pesosExchangeRate;

            return pesosAmount;
        }

        public static decimal ConvertUSDToBTC(decimal usdAmount)
        {
            // Tasa de cambio: 1 USD = 0.000033 BTC
            decimal btcExchangeRate = 0.000033m; // Utiliza 'm' al final para definir el valor como decimal

            // Realiza la conversión de dólares a BTC
            decimal btcAmount = usdAmount * btcExchangeRate;

            return btcAmount;
        }


    }
}
