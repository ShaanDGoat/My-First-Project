using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Project_DataAccess.Data;
using Project_Models;
using Project_Models.ViewModels;
using Project_Utility;
using Project_DataAccess.Repository;
using Project_DataAccess.Repository.IRepository;

namespace My_First_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IApplicationRepository _applicationTypeRepository;
        private readonly IProductRepository _productRepository;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ICategoryRepository categoryRepository, IApplicationRepository applicationTypeRepository, IProductRepository productRepository, ApplicationDbContext db)
        {
            _applicationTypeRepository = applicationTypeRepository;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _logger = logger;
            _db = db;

        }

        public IActionResult Index()
        {
            HomeVM homeVM = new()
            {
                Products = _productRepository.GetAll(includeProperties:("Category,ApplicationType")),
                Categories = _categoryRepository.GetAll()
            };
            return View(homeVM);
        }

        public IActionResult Details(int id)
        {
            List<ShoppingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)

            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }


            DetailsVM DetailsVM = new()
            {
                Product = _productRepository.FirstOrDefault(u => u.Id == id, includeProperties: "Category,ApplicationType"),
                ExistsInCart = false
            };

            foreach(var item in shoppingCartList)
            {
                if(item.ProductId == id)
                {
                    DetailsVM.ExistsInCart = true; 
                }
            }

            return View(DetailsVM);

        }

        public IActionResult RemoveFromCart(int id)
        {
            List<ShoppingCart> shoppingCartList = new();
            if (HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)

            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }

            var itemtoremove = shoppingCartList.SingleOrDefault(r => r.ProductId == id);
            if(itemtoremove != null)
            {
                shoppingCartList.Remove(itemtoremove);
            };

            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));

        }

        [HttpPost,ActionName("Details")]
        public IActionResult DetailsPost(int id, DetailsVM detailsVM)
        {
            List<ShoppingCart> shoppingCartList = new();
            if(HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart) != null
                && HttpContext.Session.Get<IEnumerable<ShoppingCart>>(WC.SessionCart).Count() > 0)

            {
                shoppingCartList = HttpContext.Session.Get<List<ShoppingCart>>(WC.SessionCart);
            }
            shoppingCartList.Add(new ShoppingCart
            {
                ProductId = id,
                SqFt = detailsVM.Product.TempSqFt
            });
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);

            return RedirectToAction(nameof(Index));

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
