using CriptoSofttek.DTOs;
using CriptoSofttek.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CriptoSofttek.Entities
{
    public class User
    {
        public User() { }

        public User(RegisterDTO dto)
        {
            FirstName = dto.FirstName;
            LastName = dto.LastName;
            Email = dto.Email;
            Password = PasswordEncryptHelper.EncryptPassword(dto.Password, dto.Email);
            Activo = true;
            FiatAccountId = 0;
            CryptoAccountId = 0;
        }

        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
        public int FiatAccountId { get; set; }
        public FiatAccount? FiatAccount { get; set; }
        public int CryptoAccountId { get; set; }
        public CryptoAccount? CryptoAccount { get; set; }

    }
}
