using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SalesWebProject.Models;
using SalesWebProject.ViewModels;
using System.Diagnostics;
using Mysqlx;
using SalesWebProject.Enums;
using SalesWebProject.Helpers;

namespace SalesWebProject.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesContext _context;
        public SalesRecordsController(SalesContext context)
        {
            _context = context;
        }

        // Organizar os view das vendas
        public ActionResult Index()
        {
            List<SalesRecordsViewModel> list = (from m in _context.SalesRecord
                                                select new SalesRecordsViewModel
                                                {
                                                    Id = m.Id,
                                                    Date = m.Date,
                                                    Amount = m.Amount,
                                                    Status = m.Status,
                                                    Seller = new Seller
                                                    {
                                                        Id = m.Seller.Id,
                                                        Name = m.Seller.Name
                                                    }

                                                }).ToList();

            return View(list);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {

            if (id == 0 || id == null)
            {
                return RedirectToAction(nameof(Error), new { message = string.Format("Identificador #{0} não encontrado", id) });

            }

            var saleRecord = _context.SalesRecord.Include(m => m.Seller).FirstOrDefault(m => m.Id == id);
            if (saleRecord == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor não encontrado" });
            }

            var salesRecordView = new SalesRecordsViewModel
            {
                Id = saleRecord.Id,
                Date = saleRecord.Date,
                Amount = saleRecord.Amount,
                Status = saleRecord.Status,
                Seller = new Seller
                {
                    Id = saleRecord.Seller.Id,
                    Name = saleRecord.Seller.Name
                }
            };

            return View(salesRecordView);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var sellers = SellersController.ListAll(_context, true);
            ViewBag.Sellers = new SelectList(sellers, "Value", "Text");
            return View();

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SalesRecordsViewModel viewModel)
        {
            if (viewModel.SellerId == 0)
            {
                return RedirectToAction(nameof(Error), new { message = "Vendedor inválido" });
            }

            var saleRecord = new SalesRecord
            {
                Date = viewModel.Date,
                Amount = viewModel.Amount,
                Status = SaleStatusEnum.Pending,
                SellerId = viewModel.SellerId,
            };

            _context.SalesRecord.Add(saleRecord);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var saleRecord = _context.SalesRecord.FirstOrDefault(m => m.Id == id);
            if (saleRecord == null)
            {
                throw new Exception("Departamento não encontrado");
            }

            var viewModel = new SalesRecordsViewModel
            {
                Id = saleRecord.Id,
                Date = saleRecord.Date,
                Amount = saleRecord.Amount,
                Status = saleRecord.Status,
                SellerId = saleRecord.SellerId,
            };

            var enums = EnumHelpers.GenerateSelectList<SaleStatusEnum>();
            ViewBag.Status = new SelectList(enums, "Value", "Text");

            var sellers = SellersController.ListAll(_context, true);
            ViewBag.Sellers = new SelectList(sellers, "Value", "Text");

            return View(viewModel);

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
