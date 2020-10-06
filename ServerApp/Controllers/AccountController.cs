using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;

namespace ServerApp.Controllers
{
	public class AccountController : Controller
	{
		private UserManager<IdentityUser> m_UserManager;
		private SignInManager<IdentityUser> m_SignInManager;

		public AccountController( UserManager<IdentityUser> userManager,
			SignInManager<IdentityUser> signInManager )
		{
			m_UserManager = userManager;
			m_SignInManager = signInManager;
		}

		#region Methods.

		//	Perform authentication.
		//	Web service authentication will also use this method.
		private async Task<bool> DoLogin( LoginViewModel loginViewModel )
		{
			Microsoft.AspNetCore.Identity.SignInResult signInResult;
			bool isDoLogin;

			IdentityUser identityUser = await m_UserManager.FindByNameAsync( loginViewModel.Name );
			if( identityUser == null )
			{
				isDoLogin = false;
			}
			else
			{
				//	Signout if already signed-in.
				await m_SignInManager.SignOutAsync();

				bool isLockoutOnFailure;
				bool isPersistent;

				isLockoutOnFailure = false;
				isPersistent = false;
				signInResult = await m_SignInManager.PasswordSignInAsync( identityUser, loginViewModel.Password, isPersistent, isLockoutOnFailure );
				isDoLogin = signInResult.Succeeded;
			}
			return ( isDoLogin );
		}
		#endregion

		//	Render a Razor view that will prompt the user for their credentials.
		[HttpGet]
		public IActionResult Login( string returnUrl )
		{
			ViewBag.returnUrl = returnUrl;
			return View();
		}

		//	Validate the credentials and sign-in the user.
		[HttpPost]
		public async Task<IActionResult> Login( LoginViewModel loginViewModel,
			string returnUrl )
		{
			if( ModelState.IsValid )
			{
				if( await DoLogin( loginViewModel ))
				{
					return ( Redirect( returnUrl ?? "/" ) );
				}
				else
				{
					ModelState.AddModelError( "", "The username and/or password do not match an existing account." );
				}
			}
			return View ( loginViewModel );
		}

		[HttpPost]
		public async Task<IActionResult> Logout( string redirectUrl )
		{
			await m_SignInManager.SignOutAsync();
			return Redirect( redirectUrl ?? "/" );
		}

		[HttpPost( "/api/account/login" )]
		public async Task<IActionResult> Login([FromBody] LoginViewModel loginViewModel)
		{
			IActionResult iActionResult;

			if( ModelState.IsValid 
				&& await DoLogin( loginViewModel ))
			{
				iActionResult = Ok( "true" );
			}
			else
			{
				iActionResult = BadRequest();
			}
			return ( iActionResult );
		}

		[HttpPost( "/api/account/logout" )]
		public async Task<IActionResult> Logout()
		{
			IActionResult iActionResult;

			await m_SignInManager.SignOutAsync();
			iActionResult = Ok();
			return ( iActionResult );
		}
	}

	public class LoginViewModel
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string Password { get; set; }
	}
}
