using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface ICityService
    {
        public IQueryable<CityQuery> Query();
        public Service Create(CityCommand city);
        public CityCommand Edit(int id);
        public Service Update(CityCommand city);
        public Service Delete(int id);

        public List<CityQuery> GetList(int? countryId);
    }

    public class CityService : Service, ICityService
    {
        public CityService(Db db) : base(db)
        {
        }

        public IQueryable<CityQuery> Query()
        {
            return _db.Cities.Include(c => c.Country).OrderBy(c => c.Name).Select(c => new CityQuery()
            {
                Id = c.Id,
                Name = c.Name,
                Country = c.Country.Name
            });
        }

        public Service Create(CityCommand city)
        {
            if (_db.Cities.Any(c => c.Name.ToUpper() == city.Name.ToUpper().Trim()))
                return Error("City with the same name exists!");
            City entity = new City()
            {
                Name = city.Name.Trim(),
                CountryId = city.CountryId
            };
            _db.Cities.Add(entity);
            _db.SaveChanges();
            city.Id = entity.Id;
            return Success("City created successfully.");
        }

        public CityCommand Edit(int id)
        {
            City entity = _db.Cities.SingleOrDefault(c => c.Id == id);
            return new CityCommand()
            {
                Id = entity.Id,
                Name = entity.Name,
                CountryId = entity.CountryId
            };
        }

        public Service Update(CityCommand city)
        {
            if (_db.Cities.Any(c => c.Id != city.Id && c.Name.ToUpper() == city.Name.ToUpper().Trim()))
                return Error("City with the same name exists!");
            City entity = _db.Cities.SingleOrDefault(c => c.Id == city.Id);
            entity.Name = city.Name.Trim();
            entity.CountryId = city.CountryId;
            _db.Cities.Update(entity);
            _db.SaveChanges();
            return Success("City updated successfully.");
        }

        public Service Delete(int id)
        {
            City entity = _db.Cities.Include(c => c.Stores).SingleOrDefault(c => c.Id == id);
            if (entity.Stores.Any())
                return Error("City has relational stores!");
            _db.Cities.Remove(entity);
            _db.SaveChanges();
            return Success("City deleted successfully.");
        }

        public List<CityQuery> GetList(int? countryId)
        {
            return countryId.HasValue ? _db.Cities.OrderBy(c => c.Name).Where(c => c.CountryId == countryId).Select(c => new CityQuery()
            {
                Id = c.Id,
                Name = c.Name
            }).ToList() : new List<CityQuery>();
        }
    }
}