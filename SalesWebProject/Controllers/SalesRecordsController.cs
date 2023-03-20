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

        public ActionResult Index(SalesRecordsMainViewModel viewModel)
        {

            SalesRecordsGroupType status = viewModel.Filter.Status;

            var tmpQuery = from m in _context.SalesRecord.AsNoTracking()
                           select new
                           {
                               m.Id,
                               m.Date,
                               m.Amount,
                               m.Status,
                               Seller = new
                               {
                                   Id = m.Seller.Id,
                                   m.Seller.Name,
                                   m.Seller.DepartmentId,
                                   DepartmentName = m.Seller.Department.Name

                               },
                           };

            if (!string.IsNullOrWhiteSpace(viewModel.Filter.Name))
            {
                tmpQuery = tmpQuery.Where(m => m.Seller.Name.Contains(viewModel.Filter.Name));
            }

            if (!viewModel.Filter.AllPeriods)
            {
                if (viewModel.Filter.DateStart != null)
                {
                    tmpQuery = tmpQuery.Where(m => m.Date >= viewModel.Filter.DateStart);
                }

                if (viewModel.Filter.DateEnd != null)
                {
                    tmpQuery = tmpQuery.Where(m => m.Date <= viewModel.Filter.DateEnd);
                }
            }

            var salesRecords = new SalesRecordsMainViewModel
            {

                SalesRecordsGroups = (from m in tmpQuery.ToList()
                                      group m by

                                      status == SalesRecordsGroupType.Department ?
                                      new { Id = m.Seller.DepartmentId, Name = m.Seller.DepartmentName } :
                                      status == SalesRecordsGroupType.Status ?
                                      new { Id = (int)m.Status, Name = m.Status.ToString() } :
                                      new { Id = m.Seller.Id, Name = m.Seller.Name }
                                      into g

                                      select new SalesRecordsGroupedViewModel
                                      {
                                          Name = status == SalesRecordsGroupType.Status ?
                                          ((SaleStatusEnum)Enum.Parse(typeof(SaleStatusEnum), g.Key.Name)).GetDescription() :
                                          g.Key.Name,

                                          Counter = g.Count(m => status == SalesRecordsGroupType.Department ?
                                          m.Seller.DepartmentId == g.Key.Id :
                                          status == SalesRecordsGroupType.Status ?
                                          (int)m.Status == g.Key.Id :
                                          m.Seller.Id == g.Key.Id),

                                          SalesRecords = (from m in g.ToList()
                                                          orderby m.Seller.Name
                                                          select new SalesRecordsViewModel
                                                          {
                                                              Id = m.Id,
                                                              Date = m.Date,
                                                              Amount = m.Amount,
                                                              Status = m.Status,
                                                              SellerName = m.Seller.Name,
                                                              DepartmentName = m.Seller.DepartmentName
                                                          }).ToList()
                                      }).ToList(),

                Filter = viewModel.Filter
            };

            return View(salesRecords);
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
        [ValidateAntiForgeryToken]
        public ActionResult Edit(SalesRecordsViewModel viewModel)
        {
            var saleRecord = _context.SalesRecord.FirstOrDefault(m => m.Id == viewModel.Id);

            if (saleRecord == null)
            {
                throw new Exception("Venda não encontrada");
            }

            saleRecord.Date = viewModel.Date;
            saleRecord.Amount = viewModel.Amount;
            saleRecord.Status = viewModel.Status;
            saleRecord.SellerId = viewModel.SellerId;


            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var saleRecord = _context.SalesRecord.Include(m => m.Seller).FirstOrDefault(m => m.Id == id);
            if (saleRecord == null)
            {
                throw new Exception("Venda não encontrada");
            }

            var viewModel = new SalesRecordsViewModel
            {
                Id = saleRecord.Id,
                Date = saleRecord.Date,
                Status = saleRecord.Status,
                Amount = saleRecord.Amount,
                SellerName = saleRecord.Seller.Name,
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(SalesRecordsViewModel viewModel)
        {
            try
            {
                var saleRecord = _context.SalesRecord.FirstOrDefault(m => m.Id == viewModel.Id);
                if (saleRecord == null)
                {
                    throw new Exception("Vendedor não encontrado");
                }
                _context.SalesRecord.Remove(saleRecord);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                return RedirectToAction(nameof(Error), new { message = "Não foi possível deletar este item em razão de chaves primárias no banco de dados" });
            }
        }

        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(i => i.Id == id);
        }
    }
}
