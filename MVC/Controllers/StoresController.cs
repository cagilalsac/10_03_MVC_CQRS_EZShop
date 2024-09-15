#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BLL.Controllers.Bases;
using BLL.Services;
using BLL.Models;
using Microsoft.AspNetCore.Authorization;

// Generated from Custom Template.

namespace MVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StoresController : MvcController
    {
        // Service injections:
        private readonly IStoreService _storeService;
        private readonly ICityService _cityService;
        private readonly ICountryService _countryService;

        /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
        //private readonly IManyToManyRecordService _ManyToManyRecordService;

        public StoresController(
			IStoreService storeService
            , ICityService cityService
            , ICountryService countryService

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //, IManyToManyRecordService ManyToManyRecordService
        )
        {
            _storeService = storeService;
            _cityService = cityService;
            _countryService = countryService;

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //_ManyToManyRecordService = ManyToManyRecordService;
        }

        // GET: Stores
        public IActionResult Index()
        {
            // Get collection service logic:
            var list = _storeService.GetList();
            return View(list);
        }

        // GET: Stores/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _storeService.GetItem(id);
            return View(item);
        }

        protected void SetViewData(int? countryId = null)
        {
            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            ViewData["CityId"] = new SelectList(_cityService.GetList(countryId), "Id", "Name");
            ViewData["CountryId"] = new SelectList(_countryService.Query().ToList(), "Id", "Name");
            
            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            //ViewBag.ManyToManyRecordIds = new MultiSelectList(_ManyToManyRecordService.Query().ToList(), "Id", "Name");
        }

        // GET: Stores/Create
        public IActionResult Create()
        {
            SetViewData();
            var store = new StoreCommand()
            {
                IsVirtual = true
            };
            return View(store);
        }

        // POST: Stores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(StoreCommand store)
        {
            if (ModelState.IsValid)
            {
                // Insert item service logic:
                var result = _storeService.Create(store);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = store.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(store);
        }

        // GET: Stores/Edit/5
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _storeService.Edit(id);
            SetViewData(item.CountryId);
            return View(item);
        }

        // POST: Stores/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(StoreCommand store)
        {
            if (ModelState.IsValid)
            {
                // Update item service logic:
                var result = _storeService.Update(store);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = store.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData(store.CountryId);
            return View(store);
        }

        // GET: Stores/Delete/5
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _storeService.GetItem(id);
            return View(item);
        }

        // POST: Stores/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var result = _storeService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
	}
}
