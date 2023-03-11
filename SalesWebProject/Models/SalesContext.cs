using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesWebProject.Models;

namespace SalesWebProject.Models
{
    public class SalesContext : DbContext
    {
        public SalesContext(DbContextOptions<SalesContext> options) : base(options) { }

        public DbSet<Department> Department { get; set; }
    }
}
