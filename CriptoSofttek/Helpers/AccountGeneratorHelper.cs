using CriptoSofttek.Services;

namespace CriptoSofttek.Helpers
{
    public class AccountGeneratorHelper
    {
        public static string GenerateAccountNumber(string email)
        {
            string accountNumber = email.Substring(0, 3) + email.Substring(email.Length - 3, 3) + email.Length.ToString();
            return accountNumber;
        }

        public static string GenerateAlias(string lastName, string email)
        {
            string alias = lastName + "." + email.Substring(0, 3) + email.Substring(email.Length - 3, 3) + new Random().Next(10000, 99999).ToString();
            return alias;
        }

        public static string GenerateCBU()
        {
            int CBU = new Random().Next(100000000, 999999999);
            return CBU.ToString();
        }



    }
}
