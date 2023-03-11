using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesWebProject.Models;
using SalesWebProject.ViewModels.DeparmentViewModel;
using System.Collections.Generic;

namespace SalesWebProject.Controllers
{
    public class DepartmentsController : Controller
    {
        private SalesContext _context;
        public DepartmentsController(SalesContext context)
        {
            this._context = context;
        }

        public ActionResult Index()
        {
            List<DepartmentViewModel> list = (from m in _context.Department
                                              select new DepartmentViewModel
                                              {
                                                  Id = m.Id,
                                                  Name = m.Name
                                              }).ToList();

            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = _context.Department.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
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

            return View(department);
        }

        public ActionResult Edit(int id)
        {
            var department = _context.Department.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var item = new DepartmentViewModel
            {
                Id = department.Id,
                Name = department.Name,
            };

            return View(item);
        }

        [HttpPost]
        public ActionResult Edit(DepartmentViewModel viewModel)
        {
            var department = _context.Department.FirstOrDefault(m => m.Id == viewModel.Id);
            if (department == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            department.Name = viewModel.Name;

            _context.SaveChanges();

            return View(viewModel);
        }

        // GET: Departments/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null || _context.Department == null)
            {
                return NotFound();
            }

            var department = _context.Department
                .FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (_context.Department == null)
            {
                return Problem();
            }
            var department = _context.Department.Find(id);
            if (department != null)
            {
                _context.Department.Remove(department);
            }

            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return _context.Department.Any(i => i.Id == id);
        }
    }
}
