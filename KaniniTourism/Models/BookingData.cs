using System.ComponentModel.DataAnnotations;

namespace KaniniTourism.Models
{
    public class BookingData
    {
        [Key]
        public int BookingId { get; set; }
        public int TravelerId { get; set; }
        public int TouristId { get; set; }
    }
}
