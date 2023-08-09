using KaniniTourism.Models;
using System.Collections.Generic;

namespace KaniniTourism.Repository
{
    public class BookingRepository : IBookingRepository
    {
        // Implement the methods defined in the IBookingRepository interface

        private readonly KaniniDbContext _context; // Your database context (replace YourDbContext with your actual DbContext class)

        public BookingRepository(KaniniDbContext context)
        {
            _context = context;
        }
        public BookingData GetBookingByTravelerId(int travelerId)
        {
            // Implement the logic to retrieve the booking based on the travelerId
            // For example:
            return _context.BookingData.FirstOrDefault(b => b.TravelerId == travelerId);
        }
        public void AddBooking(BookingData booking)
        {
            _context.BookingData.Add(booking);
            _context.SaveChanges();
        }

        public BookingData GetBookingById(int bookingId)
        {
            return _context.BookingData.Find(bookingId);
        }

        public IEnumerable<BookingData> GetAllBookings()
        {
            return _context.BookingData.ToList();
        }

        public IEnumerable<Travelers> GetTravelersByBookedTouristId(int touristId)
        {
            var bookings = _context.BookingData.Where(b => b.TouristId == touristId).ToList();
            var travelerIds = bookings.Select(b => b.TravelerId).ToList();

            return _context.Traveler.Where(t => travelerIds.Contains(t.TravelerId)).ToList();
        }



        // Implement other methods based on your requirements
    }
}
