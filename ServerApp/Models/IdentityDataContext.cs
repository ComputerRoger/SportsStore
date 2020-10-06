using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ServerApp.Models
{
	//	Provide a data context class so that it will be easier to apply the migration to setup the database schema.
	public class IdentityDataContext : IdentityDbContext<IdentityUser>
	{
		public IdentityDataContext( DbContextOptions<IdentityDataContext> options ) : base( options )
		{
		}
	}
}
