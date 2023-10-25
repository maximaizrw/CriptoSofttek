namespace CriptoSofttek.DTOs
{
    public class MovementDTO
    {
        public int UserId { get; set; } 
        public string Currency { get; set; }
        public decimal Amount { get; set; }
        public string TypeMovement { get; set; }
        public DateTime Date { get; set; }

    }
}
