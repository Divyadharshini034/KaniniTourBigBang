using Microsoft.EntityFrameworkCore;

namespace KaniniTourism.Models
{
    public class KaniniDbContext:DbContext
    {
       

        public DbSet<Travelers> Traveler { get; set; }

        public DbSet<TouristPlaces> TouristPlaces { get; set; }

       public DbSet<BookingData> BookingData { get; set; }
        public KaniniDbContext(DbContextOptions<KaniniDbContext> options)
       : base(options)
        {
        }

    }
}
