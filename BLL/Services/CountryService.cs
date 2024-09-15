using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface ICountryService
    {
        public IQueryable<CountryQuery> Query();
        public Service Create(CountryCommand country);
        public CountryCommand Edit(int id);
        public Service Update(CountryCommand country);
        public Service Delete(int id);
    }

    public class CountryService : Service, ICountryService
    {
        public CountryService(Db db) : base(db)
        {
        }

        public IQueryable<CountryQuery> Query()
        {
            return _db.Countries.OrderBy(c => c.Name).Select(c => new CountryQuery()
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public Service Create(CountryCommand country)
        {
            if (_db.Countries.Any(c => c.Name.ToUpper() == country.Name.ToUpper().Trim()))
                return Error("Country with the same name exists!");
            Country entity = new Country()
            {
                Name = country.Name.Trim()
            };
            _db.Countries.Add(entity);
            _db.SaveChanges();
            country.Id = entity.Id;
            return Success("Country created successfully.");
        }

        public CountryCommand Edit(int id)
        {
            Country entity = _db.Countries.SingleOrDefault(c => c.Id == id);
            return new CountryCommand()
            {
                Id = entity.Id,
                Name = entity.Name,
            };
        }

        public Service Update(CountryCommand country)
        {
            if (_db.Countries.Any(c => c.Id != country.Id && c.Name.ToUpper() == country.Name.ToUpper().Trim()))
                return Error("Country with the same name exists!");
            Country entity = _db.Countries.SingleOrDefault(c => c.Id == country.Id);
            entity.Name = country.Name.Trim();
            _db.Countries.Update(entity);
            _db.SaveChanges();
            return Success("Country updated successfully.");
        }

        public Service Delete(int id)
        {
            Country entity = _db.Countries.Include(c => c.Cities).Include(c => c.Stores).SingleOrDefault(c => c.Id == id);
            if (entity.Cities.Any())
                return Error("Country has relational cities!");
            if (entity.Stores.Any())
                return Error("Country has relational stores!");
            _db.Countries.Remove(entity);
            _db.SaveChanges();
            return Success("Country deleted successfully.");
        }
    }
}
