namespace GreenApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string EmailAddress { get; set; }
        public string? Role { get; set; }

        //Navigation
        public ICollection<Listing> Listings { get; set; }

        public ICollection<PurchaseHistory> PurchaseHistories { get; set; }
    }
}
