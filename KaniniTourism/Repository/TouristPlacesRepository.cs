using System.Collections.Generic;
using KaniniTourism.Models;
using KaniniTourism.Repository;
using Microsoft.EntityFrameworkCore;

public class TouristPlacesRepository : ITouristPlacesRepository
{
    private readonly KaniniDbContext _dbContext;

    public TouristPlacesRepository(KaniniDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IEnumerable<TouristPlaces> GetAll()
    {
        return _dbContext.TouristPlaces.ToList();
    }

    public TouristPlaces GetById(int id)
    {
        return _dbContext.TouristPlaces.Find(id);
    }

    public void Add(TouristPlaces entity)
    {
        _dbContext.TouristPlaces.Add(entity);
        _dbContext.SaveChanges();
    }

    public void Update(TouristPlaces entity)
    {
        _dbContext.Entry(entity).State = EntityState.Modified;
        _dbContext.SaveChanges();
    }
   
    public void Delete(int id)
    {
        var entity = _dbContext.TouristPlaces.Find(id);
        if (entity == null) return;

        _dbContext.TouristPlaces.Remove(entity);
        _dbContext.SaveChanges();
    }

    public async Task<TouristPlaces> GetTouristPlaceById(int touristId)
    {
        return await _dbContext.TouristPlaces.FirstOrDefaultAsync(tp => tp.TouristId == touristId);
    }

    public async Task<IEnumerable<TouristPlaces>> GetTouristPlacesByCriteria(
        string source, string destination, string checkIn, string checkOut)
    {
        return await _dbContext.TouristPlaces
            .Where(tp =>
                tp.Source == source &&
                tp.Destination == destination &&
                tp.CheckIn == checkIn &&
                tp.CheckOut == checkOut)
            .ToListAsync();
    }
}
