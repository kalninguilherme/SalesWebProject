using Microsoft.EntityFrameworkCore;
using SalesWebProject.Models;
using SalesWebProject.ViewModels;
using System.Linq;

namespace SalesWebProject.Services
{
    public class DepartmentService
    {
        private readonly SalesContext _context;
        public DepartmentService(SalesContext context)
        {
            _context = context;
        }

        public List<Department> FindAll()
        {
            return _context.Departments.OrderBy(x => x.Name).ToList();
        }
    }
}

