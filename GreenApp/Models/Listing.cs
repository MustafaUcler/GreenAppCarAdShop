namespace GreenApp.Models
{
    public class Listing
    {
        public int ListingId { get; set; }
        public int UserId { get; set; }
        public string Color { get; set; }
        public string Brand { get; set; }
        public string ModelName { get; set; }
        public int Milage { get; set; }
        public string FuelType { get; set; }
        public string Gearbox { get; set; }
        public int Price { get; set; }
        public int Horsepower { get; set; }
        public int ProductionYear { get; set; }
        public string Description { get; set; }

        public DateTime PublishDate { get; set; }

        public int? Active { get; set; }

        public string GUID { get; set; }

        //Navigation
        public User User { get; set; }

        public ICollection<PurchaseHistory> PurchaseHistories { get; set; }
    }
}
