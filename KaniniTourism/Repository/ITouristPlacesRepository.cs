using KaniniTourism.Models;

namespace KaniniTourism.Repository
{
    public interface ITouristPlacesRepository
    {
        Task<TouristPlaces> GetTouristPlaceById(int touristId);
        IEnumerable<TouristPlaces> GetAll();
        TouristPlaces GetById(int id);
        void Add(TouristPlaces entity);
        void Update(TouristPlaces entity);
        void Delete(int id);

        Task<IEnumerable<TouristPlaces>> GetTouristPlacesByCriteria(
           string source, string destination, string checkIn, string checkOut);

    }
}
