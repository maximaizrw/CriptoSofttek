using CriptoSofttek.DTOs;
using CriptoSofttek.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriptoSofttek.Entities
{
    public class FiatAccount
    {
        public FiatAccount() { }
        public FiatAccount(int id, string email, string lastName)
        {
            UserId = id;
            AccountNumber = AccountGeneratorHelper.GenerateAccountNumber();
            Alias = AccountGeneratorHelper.GenerateAlias(lastName, email);
            CBU = AccountGeneratorHelper.GenerateCBU();
            USDBalance = 0;
            PesosBalance = 0;
        }


        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AccountNumber { get; set; }
        public string Alias { get; set; }
        public string CBU { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal USDBalance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PesosBalance { get; set; }
    }
}
