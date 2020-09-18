using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerApp.Models
{
	//	When changing a DbContext derived class, a migration is required.
	//	dotnet ef migrations add SomeMigrationName
	//	Note:  To undo this action, use 'ef migrations remove'

	//	The database can be dropped for recreation:  dotnet ef database drop --force
	//	Then recreate the database with Server Explorer -> Data Connections -> Create new database -> EssentialApp
	//	Then in terminal:  dotnet watch run

	public class DataContext : DbContext
    {
        public DataContext( DbContextOptions<DataContext> dbContextOptions ) : base( dbContextOptions )
        {

        }

        //  Provide access to the data in the database allowing independent queries for each model type.

        public DbSet<Product> Products{ get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Rating> Ratings { get; set; }

		//	OnModelCreating is used to override the conventions used to create a database schema.
		protected override void OnModelCreating( ModelBuilder modelBuilder )
		{
			//	HasMany and WithOne methods are used to describe both sides of the one-to-many relationship.

			//	Prevent insonsistencies to cascade removal of children when deleting a parent.
			//	Limit changes to the OnDelete method.

			//	One Product has many Ratings.
			//	Deleting a Product will delete the Ratings.
			modelBuilder.Entity<Product>().HasMany<Rating>( p => p.Ratings )
				.WithOne( r => r.Product ).OnDelete( DeleteBehavior.Cascade );

			//	One Supplier has many Products.
			//	Deleting a Supplier will not delete the Product, but set the Product supplier field to NULL.
			modelBuilder.Entity<Product>().HasOne<Supplier>( p => p.Supplier )
				.WithMany( s => s.Products ).OnDelete( DeleteBehavior.SetNull );
		}
    }
}
