using Microsoft.EntityFrameworkCore;

namespace GreenApp.Models
{
    public class GreenAppContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Listing> Listings { get; set; }

        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }

        public GreenAppContext(DbContextOptions<GreenAppContext> options)
        : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between PurchaseHistory and User
            modelBuilder.Entity<PurchaseHistory>()
                .HasOne(ph => ph.User)
                .WithMany(u => u.PurchaseHistories)
                .HasForeignKey(ph => ph.UserId)
                .OnDelete(DeleteBehavior.Restrict); // Specify the OnDelete behavior

            // Configure the relationship between PurchaseHistory and Listing
            modelBuilder.Entity<PurchaseHistory>()
                .HasOne(ph => ph.Listing)
                .WithMany(l => l.PurchaseHistories)
                .HasForeignKey(ph => ph.ListingId)
                .OnDelete(DeleteBehavior.Restrict); // Specify the OnDelete behavior
        }
    }
}
