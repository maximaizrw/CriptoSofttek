using CriptoSofttek.DTOs;
using CriptoSofttek.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CriptoSofttek.Entities
{
    public class Movement
    {
        public Movement() { }

        public Movement(MovementDTO movementDTO)
        {
            UserId = movementDTO.UserId;
            Currency = movementDTO.Currency;
            Amount = movementDTO.Amount;
            TypeMovement = movementDTO.TypeMovement;
            Date = DateTime.Now;
        }


        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string TypeMovement { get; set; }
        public DateTime Date { get; set; }

      

    }
}
