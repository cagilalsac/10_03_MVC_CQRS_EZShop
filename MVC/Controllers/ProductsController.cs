﻿#nullable disable
using BLL.Controllers.Bases;
using BLL.Models;
using BLL.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

// Generated from Custom Template.

namespace MVC.Controllers
{
    [Authorize]
    public class ProductsController : MvcController
    {
        // Service injections:
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
        private readonly IStoreService _storeService;

        public ProductsController(
			IProductService productService
            , ICategoryService categoryService

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            , IStoreService storeService
        )
        {
            _productService = productService;
            _categoryService = categoryService;

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            _storeService = storeService;
        }

        // GET: Products
        [AllowAnonymous]
        public IActionResult Index(PageModel pageModel)
        {
            // Get collection service logic:
            var list = _productService.GetList(pageModel);
            ViewBag.PageModel = pageModel;
            return View(list);
        }

        // GET: Products/Details/5
        public IActionResult Details(int id)
        {
            // Get item service logic:
            var item = _productService.Query().SingleOrDefault(q => q.Id == id);
            return View(item);
        }

        protected void SetViewData()
        {
            // Related items service logic to set ViewData (Id and Name parameters may need to be changed in the SelectList constructor according to the model):
            ViewData["CategoryId"] = new SelectList(_categoryService.Query().ToList(), "Id", "Name");

            /* Can be uncommented and used for many to many relationships. ManyToManyRecord may be replaced with the related entiy name in the controller and views. */
            ViewBag.StoreIds = new MultiSelectList(_storeService.Query().ToList(), "Id", "Name");
        }

        // GET: Products/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            SetViewData();
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductCommand product)
        {
            if (ModelState.IsValid)
            {
                // Insert item service logic:
                var result = _productService.Create(product);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = product.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(product);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            // Get item to edit service logic:
            var item = _productService.Edit(id);
            SetViewData();
            return View(item);
        }

        // POST: Products/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductCommand product)
        {
            if (ModelState.IsValid)
            {
                // Update item service logic:
                var result = _productService.Update(product);
                if (result.IsSuccessful)
                {
                    TempData["Message"] = result.Message;
                    return RedirectToAction(nameof(Details), new { id = product.Id });
                }
                ModelState.AddModelError("", result.Message);
            }
            SetViewData();
            return View(product);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            // Get item to delete service logic:
            var item = _productService.Query().SingleOrDefault(q => q.Id == id);
            return View(item);
        }

        // POST: Products/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteConfirmed(int id)
        {
            // Delete item service logic:
            var result = _productService.Delete(id);
            TempData["Message"] = result.Message;
            return RedirectToAction(nameof(Index));
        }
	}
}
