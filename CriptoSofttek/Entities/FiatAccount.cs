using CriptoSofttek.DTOs;
using CriptoSofttek.Helpers;
using System.ComponentModel.DataAnnotations.Schema;

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
            CBU = AccountGeneratorHelper.GenerateCBU();
            USDBalance = 0;
            PesosBalance = 0;
        }


        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccountNumber { get; set; }
        public string Alias { get; set; }
        public string CBU { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal USDBalance { get; set; }
        [Column(TypeName = "decimal(18,4)")]
        public decimal PesosBalance { get; set; }
    }
}
