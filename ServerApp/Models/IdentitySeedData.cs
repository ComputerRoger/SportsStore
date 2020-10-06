using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ServerApp.Models
{
	public class IdentitySeedData
	{
		private const string m_AdminUserName = "admin";
		private const string m_AdminUserPassword = "MySecret123$";
		private const string m_AdminUserRole = "Administrator";

		////////////////////	Methods		///////////////////////
		
		//	The Startup class will inject the serviceProvider to access the Identity database.
		public static async Task SeedDatabase( IServiceProvider serviceProvider )
		{
			serviceProvider.GetRequiredService<IdentityDataContext>().Database.Migrate();

			//	Create helper objects to manage users and roles that can access the Identity database.
			UserManager<IdentityUser> userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
			RoleManager<IdentityRole> roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

			IdentityRole identityRole = await roleManager.FindByNameAsync( m_AdminUserRole );
			IdentityUser identityUser = await userManager.FindByNameAsync( m_AdminUserName );

			//	If the role does not exist, create it.
			if( identityRole == null )
			{
				identityRole = new IdentityRole( m_AdminUserRole );
				IdentityResult identityResult = await roleManager.CreateAsync( identityRole );
				if( identityResult.Succeeded )
				{
					//	All is good.
				}
				else
				{
					throw new Exception( "Cannot create IdentityRole: " + identityResult.Errors.FirstOrDefault() );
				}
			}

			//	If the user does not exist, create it.
			if( identityUser == null )
			{
				identityUser = new IdentityUser( m_AdminUserName );
				IdentityResult identityResult = await userManager.CreateAsync( identityUser, m_AdminUserPassword );
				if( identityResult.Succeeded )
				{
					//	All is good.
				}
				else
				{
					throw new Exception( "Cannot create IdentityUser: " + identityResult.Errors.FirstOrDefault() );
				}
			}

			//	Ensure a newly created user is associated with its role.
			if( ! await userManager.IsInRoleAsync( identityUser, m_AdminUserRole ))
			{
				IdentityResult identityResult = await userManager.AddToRoleAsync( identityUser, m_AdminUserRole );
				if( identityResult.Succeeded )
				{
					//	All is good.
				}
				else
				{
					throw new Exception( "Cannot add user to role: " + identityResult.Errors.FirstOrDefault() );
				}
			}
		}
	}
}
