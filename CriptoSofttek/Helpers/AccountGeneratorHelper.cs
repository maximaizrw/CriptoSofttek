using CriptoSofttek.Services;

namespace CriptoSofttek.Helpers
{
    public class AccountGeneratorHelper
    {
        //Generar numero de cuenta en base al email para que no se repita pero siempre sea de 17 digitos
        public static string GenerateAccountNumber(string email)
        {
            string accountNumber = email.Substring(0, 3) + email.Substring(email.Length - 3, 3) + email.Length.ToString();
            return accountNumber;
        }

        //Generar alias con el LastNamme y primera parte del email separado por un . y 5 numeros aleatorios
        public static string GenerateAlias(string lastName, string email)
        {
            string alias = lastName + "." + email.Substring(0, 3) + email.Substring(email.Length - 3, 3) + new Random().Next(10000, 99999).ToString();
            return alias;
        }

    }
}
