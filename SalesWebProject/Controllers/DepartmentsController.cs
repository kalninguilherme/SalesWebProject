using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebProject.Models;
using SalesWebProject.ViewModels;

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
            List<DepartmentsViewModel> list = (from m in _context.Departments
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

        public ActionResult Details(int id)
        {
            var department = _context.Departments.Include(m => m.Sellers).FirstOrDefault(m => m.Id == id);

            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var viewModel = new DepartmentsViewModel
            {
                Id = department.Id,
                Name = department.Name,
                Sellers = (from m in department.Sellers
                           select new SellersViewModel
                           {
                               Id = m.Id,
                               Name = m.Name
                           }).ToList()
            };

            return View(viewModel);
        }

        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Create(Department department)
        {
            if (department.Name.Count() < 2)
            {
                throw new Exception("Nome Inválido");
            }
            _context.Add(department);

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public ActionResult Edit(int id)
        {
            var department = _context.Departments.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var item = new DepartmentsViewModel
            {
                Id = department.Id,
                Name = department.Name,
            };

            return View(item);
        }

        [HttpPost]
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

            var item = new DepartmentsViewModel
            {
                Id = department.Id,
                Name = department.Name,
            };

            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (_context.Departments == null)
            {
                return Problem();
            }
            var department = _context.Departments.FirstOrDefault(m => m.Id == id);
            if (department != null)
            {
                _context.Departments.Remove(department);
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(i => i.Id == id);
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
