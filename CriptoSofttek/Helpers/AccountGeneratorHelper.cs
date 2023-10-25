using CriptoSofttek.Services;

namespace CriptoSofttek.Helpers
{
    public class AccountGeneratorHelper
    {
        public static int GenerateAccountNumber()
        {
             int accountNumber = new Random().Next(100000000, 999999999);
            return accountNumber;
            
        }

        public static string GenerateAlias(string lastName, string email)
        {
            string alias = lastName + "." + email.Substring(0, 3) + email.Substring(email.Length - 3, 3) + new Random().Next(10000, 99999).ToString();
            return alias;
        }

        public static string GenerateCBU()
        {
            //Generar CBU
            string CBU = "285" + new Random().Next(100000000, 999999999).ToString() + new Random().Next(100000000, 999999999).ToString();
            return CBU;
           
        }




    }
}
