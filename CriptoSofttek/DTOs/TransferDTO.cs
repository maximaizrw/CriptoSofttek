namespace CriptoSofttek.DTOs
{
    public class TransferDTO
    {
        public int Currency { get; set; }
        public decimal Amount { get; set; }
        public string CBUOrUUID { get; set; }
        public string CBUOrUUIDDestination { get; set; }

          
    }
}
