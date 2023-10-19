using CriptoSofttek.DTOs;
using CriptoSofttek.Helpers;

namespace CriptoSofttek.Entities
{
    public class FiatAccount
    {
        public FiatAccount() { }
        public FiatAccount(RegisterDTO dto, int id)
        {
            UserId = id;
            AccountNumber = AccountGeneratorHelper.GenerateAccountNumber(dto.Email);
            Alias = AccountGeneratorHelper.GenerateAlias(dto.LastName, dto.Email);
            USDBalance = 0;
            PesosBalance = 0;
        }


        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountNumber { get; set; }
        public string Alias { get; set; }
        public decimal USDBalance { get; set; }
        public decimal PesosBalance { get; set; }
    }
}
