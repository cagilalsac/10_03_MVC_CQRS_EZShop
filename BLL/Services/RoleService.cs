using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace BLL.Services
{
    public interface IRoleService
    {
        public IQueryable<RoleQuery> Query();
        public Service Create(RoleCommand role);
        public RoleCommand Edit(int id);
        public Service Update(RoleCommand role);
        public Service Delete(int id);
    }

    public class RoleService : Service, IRoleService
    {
        public RoleService(Db db) : base(db)
        {
        }

        public IQueryable<RoleQuery> Query()
        {
            return _db.Roles.Select(r => new RoleQuery()
            {
                Id = r.Id,
                RoleName = r.RoleName
            });
        }

        public Service Create(RoleCommand role)
        {
            if (_db.Roles.Any(r => r.RoleName.ToUpper() == role.RoleName.ToUpper().Trim()))
                return Error("Role with the same name exists!");
            var entity = new Role()
            {
                RoleName = role.RoleName.Trim()
            };
            _db.Roles.Add(entity);
            _db.SaveChanges();
            role.Id = entity.Id;
            return Success("Role created successfully.");
        }

        public RoleCommand Edit(int id)
        {
            var entity = _db.Roles.SingleOrDefault(r => r.Id == id);
            return new RoleCommand()
            {
                Id = entity.Id,
                RoleName = entity.RoleName
            };
        }

        public Service Update(RoleCommand role)
        {
            if (_db.Roles.Any(r => r.Id != role.Id && r.RoleName.ToUpper() == role.RoleName.ToUpper().Trim()))
                return Error("Role with the same name exists!");
            var entity = _db.Roles.SingleOrDefault(r => r.Id == role.Id);
            entity.RoleName = role.RoleName.Trim();
            _db.Roles.Update(entity);
            _db.SaveChanges();
            return Success("Role updated successfully.");
        }

        public Service Delete(int id)
        {
            var entity = _db.Roles.Include(r => r.Users).SingleOrDefault(r => r.Id == id);
            if (entity.Users.Any())
                return Error("Role has relational users!");
            _db.Roles.Remove(entity);
            _db.SaveChanges();
            return Success("Role deleted successfully.");
        }
    }
}
