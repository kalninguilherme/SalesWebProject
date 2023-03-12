using Microsoft.AspNetCore.Mvc;
using SalesWebProject.Models;
using SalesWebProject.ViewModels;

namespace SalesWebProject.Controllers
{
    public class SellerController : Controller
    {
        private SalesContext _context;

        public SellerController(SalesContext context)
        {
            this._context = context;
        }

        public ActionResult Index()
        {
            List<SellerViewModel> list = (from m in _context.Sellers
                                          select new SellerViewModel
                                          {
                                              Id = m.Id,
                                              Name = m.Name,
                                              Email = m.Email,
                                              BirthDate = m.BirthDate,
                                              BaseSalary = m.BaseSalary,
                                              DepartmentName = m.Department.Name.ToString()

                                          }).ToList();

            return View(list);
        }
    }
}
