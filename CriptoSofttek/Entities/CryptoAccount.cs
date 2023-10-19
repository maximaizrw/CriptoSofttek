using CriptoSofttek.DTOs;

namespace CriptoSofttek.Entities
{
    public class CryptoAccount
    {
        public CryptoAccount() { }

        public CryptoAccount(RegisterDTO dto, int id)
        {
            UserId = id;
            UUID = Guid.NewGuid().ToString();
            Balance = 0;
        }

        public int Id { get; set; }
        public int UserId { get; set; }
        public string UUID { get; set; }
        public decimal Balance { get; set; }

    }
}
