﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SalesWebProject.Models;

namespace SalesWebProject.Models
{
    public class SalesContext : DbContext
    {
        public SalesContext(DbContextOptions<SalesContext> options) : base(options) { }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<Seller> Sellers { get; set; }

        public virtual DbSet<SalesRecord> SalesRecord { get; set; }
    }
}
