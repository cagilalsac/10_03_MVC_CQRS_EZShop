#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Controllers.Bases;
using BLL.Services;
using BLL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

// Generated from Custom Template.

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : MvcController
    {
        // Service injections:
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
        //private readonly IManyToManyRecordService _ManyToManyRecordService;

        public UsersController(
			IUserService userService
            , IRoleService roleService

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //, IManyToManyRecordService ManyToManyRecordService
        )
        {
            _userService = userService;
            _roleService = roleService;

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //_ManyToManyRecordService = ManyToManyRecordService;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(UserCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Login(command);
                if (result.IsSuccessful)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid, _userService.LoggedInUser.Id.ToString()),
                        new Claim(ClaimTypes.Name, _userService.LoggedInUser.UserName),
                        new Claim(ClaimTypes.Role, _userService.LoggedInUser.Role)
                    };
                    ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(principal);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", result.Message);
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public IActionResult Register(UserCommand command)
        {
            if (ModelState.IsValid)
            {
                var result = _userService.Register(command);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Login));
                }
                ModelState.AddModelError("", result.Message);
            }
            return View();
        }

        // GET: Users
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _userService.Query().ToList();
            return View(list);
        }

        // GET: Users/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _userService.Query().SingleOrDefault(q => q.Id == id);
            return View(item);
        }

        protected void SetViewData()
        {
            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            ViewData["RoleId"] = new SelectList(_roleService.Query().ToList(), "Id", "RoleName");
            
            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //ViewBag.ManyToManyRecordIds = new MultiSelectList(_ManyToManyRecordService.Query().ToList(), "Id", "Name");
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            SetViewData();
            var user = new UserCommand()
            {
                IsActive = true,
                RoleId = (int)Roles.User
            };
            return View(user);
        }

        // POST: Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(UserCommand user)
        {
            if (ModelState.IsValid)
            {
                // Insert item service logic:
                var result = _userService.Create(user);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(user);
        }

        // GET: Users/Edit/5
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _userService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Users/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserCommand user)
        {
            if (ModelState.IsValid)
            {
                // Update item service logic:
                var result = _userService.Update(user);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = user.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(user);
        }

        // GET: Users/Delete/5
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _userService.Query().SingleOrDefault(q => q.Id == id);
            return View(item);
        }

        // POST: Users/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var result = _userService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
	}
}
