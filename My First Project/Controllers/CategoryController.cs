using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project_DataAccess.Data;
using Project_DataAccess.Repository;
using Project_DataAccess.Repository.IRepository;
using Project_Models;
using Project_Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace My_First_Project.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _catRep;

        public CategoryController(ICategoryRepository catRep)
        {
            _catRep = catRep;
        }


        public IActionResult Index()
        {
            IEnumerable<Category> objList = _catRep.GetAll();
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _catRep.Add(obj);
                _catRep.Save();
                TempData[WC.Success] = "Category created successfully";
                return RedirectToAction("Index"); 
            }
            TempData[WC.Error] = "  Category was unsuccessful";
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _catRep.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _catRep.Update(obj);
                _catRep.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var obj = _catRep.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }

            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _catRep.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _catRep.Remove(obj);
            _catRep.Save();
                return RedirectToAction("Index");
            
        }

    }
}
