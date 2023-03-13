using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebProject.Models;
using SalesWebProject.ViewModels;

namespace SalesWebProject.Controllers
{
    public class SalesRecordController : Controller
    {
        private SalesContext _context;

        public SalesRecordController(SalesContext context)
        {
            this._context = context;
        }

        public ActionResult Index()
        {
            List<SalesRecordViewModel> list = (from m in _context.SalesRecord
                                              select new SalesRecordViewModel
                                              {
                                                  Id = m.Id,
                                                  Date = m.Date,
                                                  Amount = m.Amount,
                                                  Status = m.Status,    
                                                  SellerId = m.SellerId,    
                                                  
                                              }).ToList();

            return View(list);
        }

        public ActionResult Details(int? id)
        {
            if (id == null || _context.Departments == null)
            {
                return NotFound();
            }

            var department = _context.Departments.FirstOrDefault(m => m.Id == id);
            if (department == null)
            {
                return NotFound();
            }

            return View();
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

            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var department = _context.Departments.FirstOrDefault(m => m.Id == id);
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

            var item = new DepartmentViewModel
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
            var department = _context.Departments.Find(id);
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
    }
}
