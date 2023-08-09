using System.ComponentModel.DataAnnotations;

namespace KaniniTourism.Models
{
    public class TouristPlaces
    {
        [Key]

        public int TouristId { get; set; }
        public string TouristName { get; set; }

        public string TouristDate { get; set; }

        public string CheckIn { get; set; }

        public string CheckOut { get; set; }
        public string Location { get; set; }
       public string Source { get; set; }

        public string Destination { get; set; }
        public string AgencyName { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number should be exactly 10 digits.")]
        public string MobileNo { get; set; }

        public int Price { get; set; }
        public string ImageData { get; set; }
       
    }
}
