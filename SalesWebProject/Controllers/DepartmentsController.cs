using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mysqlx;
using SalesWebProject.Models;
using SalesWebProject.ViewModels;
using System.Linq.Expressions;

namespace SalesWebProject.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SalesContext _context;
        public DepartmentsController(SalesContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            List<DepartmentsViewModel> list = (from m in _context.Departments.AsNoTracking()
                                               orderby m.Id
                                               select new DepartmentsViewModel
                                               {
                                                   Id = m.Id,
                                                   Name = m.Name,
                                                   Sellers = (from i in m.Sellers
                                                              select new SellersViewModel
                                                              {
                                                                  Id = i.Id,
                                                                  Name = i.Name
                                                              }).ToList()
                                               }).ToList();

            return View(list);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DepartmentsViewModel viewModel)
        {
            var department = new Department
            {
                Name = viewModel.Name,
            };

            _context.Add(department);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var department = _context.Departments.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var viewModel = new DepartmentsViewModel
            {
                Id = department.Id,
                Name = department.Name,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DepartmentsViewModel viewModel)
        {
            var department = _context.Departments.FirstOrDefault(m => m.Id == viewModel.Id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            department.Name = viewModel.Name;

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int? id)
        {
            var department = _context.Departments.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var viewModel = new DepartmentsViewModel
            {
                Id = department.Id,
                Name = department.Name,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(DepartmentsViewModel viewModel)
        {
            try
            {
                var department = _context.Departments.FirstOrDefault(m => m.Id == viewModel.Id);
                if (department == null)
                {
                    throw new Exception("Vendedor não encontrado");
                }

                _context.Departments.Remove(department);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Error), new { message = string.Format("Não foi possível deletar este item em razão de chaves primárias no banco de dados") });
            }
        }

        public static List<SelectListItem> ListAll(SalesContext context, bool addEmpty)
        {
            List<SelectListItem> departments = (from m in context.Departments
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
