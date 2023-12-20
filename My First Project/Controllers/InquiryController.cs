using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Project_DataAccess.Repository.IRepository;
using Project_Models.ViewModels;
using Project_Models;
using Project_Utility;
using Microsoft.AspNetCore.Authorization;

namespace My_First_Project.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class InquiryController : Controller
    {
        private readonly IInquiryDetailRepository _drepo;
        private readonly IInquiryHeaderRepository _Hrepo;
        
        [BindProperty]
        public InquiryVM InquiryVM { get; set; }


        public InquiryController(IInquiryHeaderRepository ih, IInquiryDetailRepository id)
        {
            _drepo = id;
            _Hrepo = ih;
        }

        public IActionResult Index()
        {
            IEnumerable<InquiryHeader> inquiries = _Hrepo.GetAll();
            return View(inquiries);
        }
 
        public IActionResult Details(int id)
        {
            InquiryVM = new InquiryVM()
            {
                inquiryHeader = _Hrepo.FirstOrDefault(u => id == u.Id),
                inquiryDetails = _drepo.GetAll(u => u.InquiryHeaderId == id, includeProperties: "Product")
            };
            return View(InquiryVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details()
        {
            List<ShoppingCart> shoppingCartList = new List<ShoppingCart>();
            InquiryVM.inquiryDetails = _drepo.GetAll(u => u.InquiryHeaderId == InquiryVM.inquiryHeader.Id);

            foreach(var detail in InquiryVM.inquiryDetails)
            {
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    ProductId = detail.ProductId
                    
                };
                shoppingCartList.Add(shoppingCart);
            }
            HttpContext.Session.Clear();
            HttpContext.Session.Set(WC.SessionCart, shoppingCartList);
            HttpContext.Session.Set(WC.SessionInquiryId, InquiryVM.inquiryHeader.Id);
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public IActionResult Delete()
        {
            InquiryHeader inquiryHeader = _Hrepo.FirstOrDefault(u => u.Id == InquiryVM.inquiryHeader.Id);
            IEnumerable<InquiryDetail> inquiryDetails = _drepo.GetAll(u => u.InquiryHeaderId == InquiryVM.inquiryHeader.Id);

            _drepo.RemoveRange(inquiryDetails);
            _Hrepo.Remove(inquiryHeader);
            _Hrepo.Save();
            return RedirectToAction(nameof(Index));
        }
        
        #region API CALLS
        [HttpGet]
        public IActionResult GetInquiryList()
        {
            return Json(new { data = _Hrepo.GetAll() });
        }
        #endregion
    }
}
