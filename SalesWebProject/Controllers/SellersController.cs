using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using SalesWebProject.Models;
using SalesWebProject.ViewModels;
using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace SalesWebProject.Controllers
{
    public class SellersController : Controller
    {
        private readonly SalesContext _context;
        public SellersController(SalesContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {

            List<SellersMainViewModel> groupedSellers = (from m in _context.Sellers.AsNoTracking()
                                                         group m by new { m.Department.Id, m.Department.Name } into g
                                                         orderby g.Key.Name
                                                         select new SellersMainViewModel
                                                         {
                                                             Name = g.Key.Name,
                                                             Counter = g.Count(i => i.DepartmentId == g.Key.Id),
                                                             SumSalary = g.Sum(i => i.BaseSalary),
                                                             Sellers = (from m in g.ToList()
                                                                        orderby m.Name
                                                                        select new SellersViewModel
                                                                        {
                                                                            Id = m.Id,
                                                                            Name = m.Name,
                                                                            Email = m.Email,
                                                                            BirthDate = m.BirthDate,
                                                                            BaseSalary = m.BaseSalary,
                                                                            Department = m.Department
                                                                        }).ToList()
                                                         }).ToList();
            return View(groupedSellers);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return RedirectToAction(nameof(Error), new { message = string.Format("Identificador #{0} não encontrado", id) });

            }

            var seller = _context.Sellers.Include(m => m.Department).FirstOrDefault(m => m.Id == id);
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor não encontrado" });
            }

            var sellersView = new SellersViewModel
            {
                Id = seller.Id,
                Name = seller.Name,
                Email = seller.Email,
                BirthDate = seller.BirthDate,
                BaseSalary = seller.BaseSalary,
                Department = seller.Department,
            };

            return View(sellersView);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var departments = DepartmentsController.ListAll(_context, true);
            ViewBag.Departments = new SelectList(departments, "Value", "Text", departments.FirstOrDefault());
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SellersViewModel viewModel)
        {

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
            if (id == 0)
            {
                return RedirectToAction(nameof(Error), new { message = string.Format("Identificador #{0} não encontrado", id) });

            }

            var seller = _context.Sellers.Include(m => m.Department).FirstOrDefault(m => m.Id == id);
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor não fornecido" });
            }

            var item = new SellersViewModel
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SellersViewModel viewModel)
        {
            var seller = _context.Sellers.FirstOrDefault(m => m.Id == viewModel.Id);
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor não fornecido" });
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
                return RedirectToAction(nameof(Error), new { message = string.Format("Identificador #{0} não encontrado", id) });

            }

            var seller = _context.Sellers.Include(m => m.Department).FirstOrDefault(m => m.Id == id);
            if (seller == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor não fornecido" });
            }

            var viewModel = new SellersViewModel
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
        public ActionResult Delete(SellersViewModel viewModel)
        {
            try
            {
                var seller = _context.Sellers.FirstOrDefault(m => m.Id == viewModel.Id);
                if (seller == null)
                {
                    throw new Exception("Vendedor não encontrado");
                }

                _context.Sellers.Remove(seller);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Error), new { message = string.Format("Não foi possível deletar este item em razão de chaves primárias no banco de dados") });
            }
        }

        public static List<SelectListItem> ListAll(SalesContext context, bool addEmpty)
        {
            var departments = (from m in context.Sellers
                               select new SelectListItem
                               {
                                   Text = m.Name,
                                   Value = m.Id.ToString()
                               }).ToList();
            if (addEmpty)
            {
                departments.Insert(0, new SelectListItem
                {
                    Text = "Selecione",
                    Value = "0"
                });
            }
            return departments;
        }

    }
}
