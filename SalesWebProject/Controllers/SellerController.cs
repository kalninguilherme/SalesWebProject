using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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
            List<SellerMainViewModel> list = (from m in _context.Sellers
                                              group m by new { m.Department.Id, m.Department.Name } into g
                                              orderby g.Key.Name
                                              select new SellerMainViewModel
                                              {
                                                  Name = g.Key.Name,
                                                  Counter = g.Count(i => i.DepartmentId == g.Key.Id),

                                                  Sellers = (from m in g.ToList()
                                                             orderby m.Name
                                                             select new SellerViewModel
                                                             {
                                                                 Id = m.Id,
                                                                 Name = m.Name,
                                                                 Email = m.Email,
                                                                 BirthDate = m.BirthDate,
                                                                 BaseSalary = m.BaseSalary,
                                                                 Department = m.Department
                                                             }).ToList()
                                              }).ToList();

            List<SellerMainViewModel> a = list.Skip(2).Take(3).ToList();

            return View(a);
        }

        public ActionResult Details(int id)
        {
            var seller = _context.Sellers.Include(m => m.Department).FirstOrDefault(m => m.Id == id);

            if (seller == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var viewModel = new SellerViewModel
            {
                Id = seller.Id,
                Name = seller.Name,
                Email = seller.Email,
                BirthDate = seller.BirthDate,
                BaseSalary = seller.BaseSalary,
                Department = seller.Department,
            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var departments = DepartmentsController.ListAll(_context, true);
            ViewBag.Departments = new SelectList(departments, "Value", "Text", departments.FirstOrDefault());
            return View();
        }

        [HttpPost]
        public ActionResult Create(SellerViewModel viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.Name))
            {
                throw new Exception("Nome não informado");
            }

            if (string.IsNullOrWhiteSpace(viewModel.Email))
            {
                throw new Exception("E-mail não informado");
            }

            var seller = new Seller
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                BirthDate = viewModel.BirthDate,
                BaseSalary = viewModel.BaseSalary,
                DepartmentId = viewModel.DepartmentId,
            };

            _context.Add(seller);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var seller = _context.Sellers.Include(m => m.Department).FirstOrDefault(m => m.Id == id);
            if (seller == null)
            {
                throw new Exception("Vendedor não encontrado");
            }

            var item = new SellerViewModel
            {
                Id = seller.Id,
                Name = seller.Name,
                Email = seller.Email,
                BirthDate = seller.BirthDate,
                BaseSalary = seller.BaseSalary,
                DepartmentId = seller.DepartmentId,
            };

            var departments = DepartmentsController.ListAll(_context, true);
            ViewBag.Departments = new SelectList(departments, "Value", "Text", departments.FirstOrDefault());

            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(SellerViewModel viewModel)
        {
            var seller = _context.Sellers.FirstOrDefault(m => m.Id == viewModel.Id);
            if (seller == null)
            {
                throw new Exception("Vendedor não encontrado");
            }

            seller.Name = viewModel.Name;
            seller.Email = viewModel.Email;
            seller.BirthDate = viewModel.BirthDate;
            seller.BaseSalary = viewModel.BaseSalary;
            seller.DepartmentId = viewModel.DepartmentId;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                throw new Exception(string.Format("Identificador #{0} não encontrado", id));
            }

            var seller = _context.Sellers.Include(m => m.Department).FirstOrDefault(m => m.Id == id);
            if (seller == null)
            {
                throw new Exception("Vendedor não encontrado");
            }

            var viewModel = new SellerViewModel
            {
                Id = seller.Id,
                Name = seller.Name,
                Email = seller.Email,
                BaseSalary = seller.BaseSalary,
                Department = seller.Department
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SellerViewModel viewModel)
        {
            var seller = _context.Sellers.FirstOrDefault(m => m.Id == viewModel.Id);
            if (seller != null)
            {
                _context.Sellers.Remove(seller);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
