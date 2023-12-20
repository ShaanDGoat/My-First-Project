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
    public class ApplicationController : Controller
    {
        private readonly IApplicationRepository _apptyperepo;



        public ApplicationController(IApplicationRepository db)
        {
            _apptyperepo = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _apptyperepo.GetAll();
            return View(objList);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateModel(ApplicationType obj)
        {
            if (ModelState.IsValid == true)
            {
                _apptyperepo.Add(obj);
                _apptyperepo.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == 0 || id < 0)
            {
                return NotFound();
            }
            var obj = _apptyperepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid == true)
            {
                _apptyperepo.Update(obj);
                _apptyperepo.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == 0 || id < 0)
            {
                return NotFound();
            }
            var obj = _apptyperepo.Find(id.GetValueOrDefault());
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _apptyperepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            _apptyperepo.Remove(obj);
            _apptyperepo.Save();
            return RedirectToAction("Index");
        }

    }
}
