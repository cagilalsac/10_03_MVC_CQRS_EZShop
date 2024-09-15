using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface ICategoryService
    {
        public IQueryable<CategoryQuery> Query();
        public Service Create(CategoryCommand category);
        public CategoryCommand Edit(int id);
        public Service Update(CategoryCommand category);
        public Service Delete(int id);
    }

    public class CategoryService : Service, ICategoryService
    {
        public CategoryService(Db db) : base(db)
        {
        }

        public IQueryable<CategoryQuery> Query()
        {
            return _db.Categories.OrderBy(c => c.Name).Select(c => new CategoryQuery()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            });
        }

        public Service Create(CategoryCommand category)
        {
            if (_db.Categories.Any(c => c.Name == category.Name.Trim()))
                return Error("Category with the same name exists!");
            Category entity = new Category()
            {
                Name = category.Name.Trim(),
                Description = category.Description?.Trim()
            };
            _db.Categories.Add(entity);
            _db.SaveChanges();
            category.Id = entity.Id;
            return Success("Category created successfully.");
        }

        public CategoryCommand Edit(int id)
        {
            Category entity = _db.Categories.Find(id);
            return new CategoryCommand()
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description
            };
        }

        public Service Update(CategoryCommand category)
        {
            if (_db.Categories.Any(c => c.Id != category.Id && c.Name == category.Name.Trim()))
                return Error("Category with the same name exists!");
            Category entity = _db.Categories.Find(category.Id);
            entity.Name = category.Name.Trim();
            entity.Description = category.Description?.Trim();
            _db.Categories.Update(entity);
            _db.SaveChanges();
            return Success("Category updated successfully.");
        }

        public Service Delete(int id)
        {
            Category entity = _db.Categories.Include(c => c.Products).FirstOrDefault(c => c.Id == id);
            if (entity.Products.Count > 0)
                return Error("Category has relational products!");
            _db.Categories.Remove(entity);
            _db.SaveChanges();
            return Success("Category deleted successfully.");
        }
    }
}
