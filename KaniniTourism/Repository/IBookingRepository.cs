using KaniniTourism.Models;

namespace KaniniTourism.Repository
{
    public interface IBookingRepository
    {
        void AddBooking(BookingData booking);

        // To retrieve a booking by its ID:
        BookingData GetBookingById(int bookingId);

        // To retrieve all bookings:
        IEnumerable<BookingData> GetAllBookings();

        BookingData GetBookingByTravelerId(int travelerId);

        IEnumerable<Travelers> GetTravelersByBookedTouristId(int touristId);
    }
}
