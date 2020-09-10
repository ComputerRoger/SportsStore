using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models
{
    public class DataContext : DbContext
    {
        public DataContext( DbContextOptions<DataContext> dbContextOptions ) : base( dbContextOptions )
        {

        }

        //  Provide access to the data in the database allowing independent queries for each model type.

        public DbSet<Product> Products{ get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Rating> Ratings { get; set; }
    }
}
