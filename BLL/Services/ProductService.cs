using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface IProductService
    {
        public IQueryable<ProductQuery> Query();
        public Service Create(ProductCommand product);
        public ProductCommand Edit(int id);
        public Service Update(ProductCommand product);
        public Service Delete(int id);

        public List<ProductQuery> GetList(PageModel pageModel);
    }

    public class ProductService : Service, IProductService
    {
        public ProductService(Db db) : base(db)
        {
        }

        public IQueryable<ProductQuery> Query()
        {
            return _db.Products.Include(p => p.Category).Include(p => p.ProductStores).ThenInclude(ps => ps.Store)
                .OrderBy(p => p.StockAmount).ThenByDescending(p => p.UnitPrice).ThenBy(p => p.Name).Select(p => new ProductQuery()
                {
                    Id = p.Id,
                    Name = p.Name,
                    UnitPrice = p.UnitPrice.ToString("C2"),
                    StockAmount = p.StockAmount.HasValue ?
                            ("<span class=" +
                            (p.StockAmount.Value < 10 ? "\"badge bg-danger\">"
                            : p.StockAmount.Value < 100 ? "\"badge bg-warning\">"
                            : "\"badge bg-success\">") + p.StockAmount.Value + "</span>") 
                        : string.Empty,
                    ExpirationDate = p.ExpirationDate.HasValue ? p.ExpirationDate.Value.ToShortDateString() : "",
                    Category = p.Category.Name,
                    Stores = string.Join("<br>", p.ProductStores.OrderBy(ps => ps.Store.Name).Select(ps => ps.Store.Name))
                });
        }

        public Service Create(ProductCommand product)
        {
            if ((product.StockAmount ?? 0) < 0)
                return Error("Stock amount must be 0 or a positive number!");
            if (product.UnitPrice <= 0 || product.UnitPrice > 100000)
                return Error("Unit price must be greater than 0 and less than 100000!");
            if (_db.Products.Any(p => p.Name.ToUpper() == product.Name.ToUpper().Trim()))
                return Error("Product with the same name exists!");
            var entity = new Product()
            {
                Name = product.Name.Trim(),
                UnitPrice = product.UnitPrice ?? 0,
                StockAmount = product.StockAmount,
                ExpirationDate = product.ExpirationDate,
                CategoryId = product.CategoryId ?? 0,
                ProductStores = product.StoreIds?.Select(sId => new ProductStore() { StoreId = sId }).ToList()
            };
            _db.Products.Add(entity);
            _db.SaveChanges();
            product.Id = entity.Id;
            return Success("Product created successfully.");
        }

        public ProductCommand Edit(int id)
        {
            var entity = _db.Products.Include(p => p.ProductStores).SingleOrDefault(p => p.Id == id);
            if (entity is null)
                return null;
            return new ProductCommand()
            { 
                Id = entity.Id,
                Name = entity.Name,
                UnitPrice = entity.UnitPrice,
                StockAmount = entity.StockAmount,
                ExpirationDate = entity.ExpirationDate,
                CategoryId = entity.CategoryId,
                StoreIds = entity.ProductStores.Select(ps => ps.StoreId).ToList()
            };
        }

        public Service Update(ProductCommand product)
        {
            if ((product.StockAmount ?? 0) < 0)
                return Error("Stock amount must be 0 or a positive number!");
            if (product.UnitPrice <= 0 || product.UnitPrice > 100000)
                return Error("Unit price must be greater than 0 and less than 100000!");
            if (_db.Products.Any(p => p.Id != product.Id && p.Name.ToUpper() == product.Name.ToUpper().Trim()))
                return Error("Product with the same name exists!");
            var entity = _db.Products.Include(p => p.ProductStores).SingleOrDefault(p => p.Id == product.Id);
            if (entity is null)
                return Error("Product not found!");
            _db.ProductStores.RemoveRange(entity.ProductStores);
            entity.Name = product.Name.Trim();
            entity.UnitPrice = product.UnitPrice ?? 0;
            entity.StockAmount = product.StockAmount;
            entity.ExpirationDate = product.ExpirationDate;
            entity.CategoryId = product.CategoryId ?? 0;
            entity.ProductStores = product.StoreIds?.Select(sId => new ProductStore() { StoreId = sId }).ToList();
            _db.Products.Update(entity);
            _db.SaveChanges();
            return Success("Product updated successfully.");
        }

        public Service Delete(int id)
        {
            var entity = _db.Products.Include(p => p.ProductStores).SingleOrDefault(p => p.Id == id);
            if (entity is null)
                return Error("Product not found!");
            _db.ProductStores.RemoveRange(entity.ProductStores);
            _db.Products.Remove(entity);
            _db.SaveChanges();
            return Success("Product deleted successfully.");
        }

        public List<ProductQuery> GetList(PageModel pageModel)
        {
            var query = Query();
            pageModel.TotalRecordsCount = query.Count();
            int recordsPerPageCount;
            if (int.TryParse(pageModel.RecordsPerPageCount, out recordsPerPageCount))
                query = query.Skip((pageModel.PageNumber - 1) * recordsPerPageCount).Take(recordsPerPageCount);
            return query.ToList();
        }
    }
}
