using CriptoSofttek.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace CriptoSofttek.Entities
{
    public class CryptoAccount
    {
        public CryptoAccount() { }

        public CryptoAccount(int id)
        {
            UserId = id;
            UUID = Guid.NewGuid().ToString();
            Balance = 0;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UUID { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal Balance { get; set; }

    }
}
