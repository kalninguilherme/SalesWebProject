using Microsoft.EntityFrameworkCore;
using SalesWebProject.Models;
using SalesWebProject.ViewModels;
using SalesWebProject.Services.Exceptions;

namespace SalesWebProject.Services
{
    public class SellerService
    {
        private readonly SalesContext _context;
        public SellerService(SalesContext context)
        {
            _context = context;
        }

        public List<SellerMainViewModel> GetSellers()
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
            return list;
        }

        //Cópia dos valores segundo o curso - Programa funciona de modo diferente

        /*public Seller FindById(int id)
        {
            return _context.Sellers.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id);
        }

        public void Remove(int id)
        {
            var obj = _context.Sellers.Find(id);
            _context.Sellers.Remove(obj);
            _context.SaveChanges();
        }

        public void Update(Seller obj)
        {
            if (!_context.Sellers.Any(x => x.Id == obj.Id))
            {
                throw new NotFoundException("Id não encontrado");
            }

            try
            {
                _context.Update(obj);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException e)
            {
                throw new DbConcurrentyException(e.Message);
            }
        }*/
    }

}
