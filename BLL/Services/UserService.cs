using BLL.DAL;
using BLL.Models;
using BLL.Services.Bases;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public interface IUserService
    {
        public UserQuery LoggedInUser { get; set; }
        public Service Register(UserCommand user);
        public Service Login(UserCommand user);

        public IQueryable<UserQuery> Query();
        public Service Create(UserCommand user);
        public UserCommand Edit(int id);
        public Service Update(UserCommand user);
        public Service Delete(int id);
    }

    public class UserService : Service, IUserService
    {
        public UserQuery LoggedInUser { get; set; }

        public UserService(Db db) : base(db)
        {
        }

        public Service Register(UserCommand user)
        {
            if (_db.Users.Any(u => u.UserName == user.UserName.Trim()))
                return Error("User with the same user name exists!");
            var entity = new User()
            {
                UserName = user.UserName.Trim(),
                Password = user.Password.Trim(),
                IsActive = true,
                RoleId = (int)Roles.User
            };
            _db.Users.Add(entity);
            _db.SaveChanges();
            return Success("User registered successfully.");
        }

        public Service Login(UserCommand user)
        {
            var entity = _db.Users.Include(u => u.Role).SingleOrDefault(u => u.UserName == user.UserName && u.Password == user.Password && u.IsActive);
            if (entity is null)
                return Error("Invalid user name or password!");
            LoggedInUser = new UserQuery()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Role = entity.Role.RoleName
            };
            return Success("User logged in successfully.");
        }

        public IQueryable<UserQuery> Query()
        {
            return _db.Users.Include(u => u.Role).OrderByDescending(u => u.IsActive).ThenBy(u => u.UserName).Select(u => new UserQuery()
            {
                Id = u.Id,
                UserName = u.UserName,
                Password = u.Password,
                IsActive = u.IsActive ? "Active": "Not Active",
                Role = u.Role.RoleName
            });
        }

        public Service Create(UserCommand user)
        {
            if (_db.Users.Any(u => u.UserName == user.UserName.Trim()))
                return Error("User with the same user name exists!");
            var entity = new User()
            {
                UserName = user.UserName.Trim(),
                Password = user.Password.Trim(),
                IsActive = user.IsActive,
                RoleId = user.RoleId
            };
            _db.Users.Add(entity);
            _db.SaveChanges();
            user.Id = entity.Id;
            return Success("User created successfully.");
        }

        public UserCommand Edit(int id)
        {
            var entity = _db.Users.SingleOrDefault(u => u.Id == id);
            return new UserCommand()
            {
                Id = entity.Id,
                UserName = entity.UserName,
                Password = entity.Password,
                IsActive = entity.IsActive,
                RoleId = entity.RoleId
            };
        }

        public Service Update(UserCommand user)
        {
            if (_db.Users.Any(u => u.Id != user.Id && u.UserName == user.UserName.Trim()))
                return Error("User with the same user name exists!");
            var entity = _db.Users.SingleOrDefault(u => u.Id == user.Id);
            entity.UserName = user.UserName.Trim();
            entity.Password = user.Password.Trim();
            entity.IsActive = user.IsActive;
            entity.RoleId = user.RoleId;
            _db.Users.Update(entity);
            _db.SaveChanges();
            return Success("User updated successfully.");
        }

        public Service Delete(int id)
        {
            var userCommand = Edit(id);
            userCommand.IsActive = false;
            var result = Update(userCommand);
            if (!result.IsSuccessful)
                return result;
            return Success("User deleted successfully.");
        }
    }
}
