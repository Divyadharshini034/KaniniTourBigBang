using KaniniTourism.Models;

namespace KaniniTourism.Repository
{
    public interface ITravelerRepository
    {
        List<Travelers> GetAllTravelers();

        Travelers GetTravelerByName(string travelerName);
        Task<Travelers> GetTravelerByNameAndPassword(string travelerName, string travelerPass);
        IEnumerable<Travelers> GetAll();
        Travelers GetById(int id);


        void Add(Travelers entity);
        void Update(Travelers entity);
        void Delete(int id);

        Travelers GetTravelerById(int travelerId);
    }
}
