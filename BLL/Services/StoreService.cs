using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface IStoreService
    {
        public IQueryable<StoreQuery> Query();
        public Service Create(StoreCommand store);
        public StoreCommand Edit(int id);
        public Service Update(StoreCommand store);
        public Service Delete(int id);

        public List<StoreQuery> GetList() => Query().ToList();
        public StoreQuery GetItem(int id) => Query().SingleOrDefault(q => q.Id == id);
    }

    public class StoreService : Service, IStoreService
    {
        protected override string OperationFailed => "Invalid operation!";

        public StoreService(Db db) : base(db)
        {
        }

        public IQueryable<StoreQuery> Query()
        {
            return _db.Stores.Include(s => s.ProductStores).ThenInclude(ps => ps.Product).Include(s => s.Country).Include(s => s.City)
                .OrderByDescending(s => s.IsVirtual).ThenBy(s => s.Name).Select(s => new StoreQuery()
            {
                Id = s.Id,
                Name = s.Name,
                IsVirtual = s.IsVirtual ? "Yes" : "No",
                ProductCount = s.ProductStores.Count.ToString(),
                Products = string.Join("<br>", s.ProductStores.Select(ps => ps.Product.Name)),
                Country = s.Country.Name,
                City = s.City.Name
            });
        }

        public Service Create(StoreCommand store)
        {
            if (_db.Stores.Any(s => s.Name.ToUpper() == store.Name.ToUpper().Trim() && s.IsVirtual == store.IsVirtual))
                return Error("Store with the same name exists!");
            var entity = new Store()
            {
                Name = store.Name.Trim(),
                IsVirtual = store.IsVirtual,
                CountryId = store.CountryId,
                CityId = store.CityId
            };
            _db.Add(entity);
            _db.SaveChanges();
            store.Id = entity.Id;
            return Success("Store created successfully.");
        }

        public StoreCommand Edit(int id)
        {
            var entity = _db.Stores.SingleOrDefault(s => s.Id == id);
            return new StoreCommand()
            {
                Id = entity.Id,
                Name = entity.Name,
                IsVirtual = entity.IsVirtual,
                CountryId = entity.CountryId,
                CityId = entity.CityId
            };
        }

        public Service Update(StoreCommand store)
        {
            if (_db.Stores.Any(s => s.Id != store.Id && s.Name.ToUpper() == store.Name.ToUpper().Trim() && s.IsVirtual == store.IsVirtual))
                return Error("Store with the same name exists!");
            var entity = _db.Stores.SingleOrDefault(s => s.Id == store.Id);
            entity.Name = store.Name.Trim();
            entity.IsVirtual = store.IsVirtual;
            entity.CountryId = store.CountryId;
            entity.CityId = store.CityId;
            _db.Update(entity);
            _db.SaveChanges();
            return Success("Store updated successfully.");
        }

        public Service Delete(int id)
        {
            var entity = _db.Stores.Include(s => s.ProductStores).SingleOrDefault(s => s.Id == id);
            _db.ProductStores.RemoveRange(entity.ProductStores);
            _db.Remove(entity);
            _db.SaveChanges();
            return Success("Store deleted successfully.");
        }
    }
}
