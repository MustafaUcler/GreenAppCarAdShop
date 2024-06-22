namespace GreenApp.Models
{
    public class PurchaseHistory
    {
        public int PurchaseHistoryId { get; set; }
        public int UserId { get; set; }
        public int ListingId { get; set; }

        //Navigation
        public User User { get; set; }
        public Listing Listing { get; set; }
    }
}
