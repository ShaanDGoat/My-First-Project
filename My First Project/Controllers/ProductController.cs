using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Project_DataAccess.Data;
using Project_DataAccess.Repository;
using Project_DataAccess.Repository.IRepository;
using Project_Models;
using Project_Models.ViewModels;
using Project_Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace My_First_Project.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepo;
        private readonly IWebHostEnvironment _IWebHostEnvironment;

        public ProductController(IProductRepository db, IWebHostEnvironment IWebHostEnvironment)
        {
            _productRepo = db;
            _IWebHostEnvironment = IWebHostEnvironment;
        }


        public IActionResult Index()
        {
            IEnumerable<Product> objList = _productRepo.GetAll(includeProperties:"Category,ApplicationType"); //Eager Loading
            return View(objList);
        }

        public IActionResult Upsert(int? id)
        {


            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _productRepo.GetAllDropDownList(WC.CategoryName),
                ApplicationTypeSelectList = _productRepo.GetAllDropDownList(WC.ApplicationTypeName)
            };


            if(id == null)
            {
                return View(productVM);
            }
            else
            {
                productVM.Product = _productRepo.Find(id.GetValueOrDefault());
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                else
                {
                    return View(productVM);
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                string webRootPath = _IWebHostEnvironment.WebRootPath;

                if (productVM.Product.Id == 0)
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }

                    productVM.Product.Image = fileName + extension;

                    _productRepo.Add(productVM.Product);

                }
                else
                {
                    //updating
                    var objFromDb = _productRepo.FirstOrDefault(u => u.Id == productVM.Product.Id, isTracking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);

                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }

                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        productVM.Product.Image = fileName + extension;

                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _productRepo.Update(productVM.Product);
                }
                _productRepo.Save();
                return RedirectToAction("Index");
            }
            productVM.CategorySelectList = _productRepo.GetAllDropDownList(WC.CategoryName);
            productVM.ApplicationTypeSelectList = _productRepo.GetAllDropDownList(WC.ApplicationTypeName);
            return View(productVM);
        }

        public IActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }
            Product product = _productRepo.GetAll(includeProperties:"Category,ApplicationType").FirstOrDefault(u => u.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _productRepo.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            string upload = _IWebHostEnvironment.WebRootPath + WC.ImagePath;

            var oldFile = Path.Combine(upload, obj.Image);

            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }

            _productRepo.Remove(obj);
            _productRepo.Save();
            return RedirectToAction("Index");

            
        }

    }
}
