using Humanizer;
using KaniniTourism.Models;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace KaniniTourism.Repository
{
    public class TravelersRepository : ITravelerRepository
    {
        private readonly KaniniDbContext _dbContext;

        public TravelersRepository(KaniniDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Travelers> GetAll()
        {
            return _dbContext.Traveler.ToList();
        }
        public Travelers GetTravelerById(int travelerId)
        {
            return _dbContext.Traveler.Find(travelerId);
        }
       
        public Travelers GetById(int id)
        {
            return _dbContext.Traveler.Find(id);
        }

        public void Add(Travelers entity)
        {
            _dbContext.Traveler.Add(entity);
            _dbContext.SaveChanges();
        }
        public async Task<Travelers> GetTravelerByNameAndPassword(string travelerName, string travelerPass)
        {
            return await _dbContext.Traveler.FirstOrDefaultAsync(u => u.TravelerName == travelerName && u.TravelerPass == travelerPass);
        }
        public List<Travelers> GetAllTravelers()
        {
            return _dbContext.Traveler.ToList();
        }
        public void Update(Travelers entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
       
        public void Delete(int id)
        {
            var entity = _dbContext.Traveler.Find(id);
            if (entity == null) return;

            _dbContext.Traveler.Remove(entity);
            _dbContext.SaveChanges();
        }

        public Travelers GetTravelerByName(string travelerName)
        {
            return _dbContext.Traveler.FirstOrDefault(t => t.TravelerName == travelerName);
        }

        public Travelers GetByTravelerName(string name)
        {
            return _dbContext.Traveler.FirstOrDefault(t => t.TravelerName == name);
        }
    }

}
