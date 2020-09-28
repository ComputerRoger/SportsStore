using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ServerApp.Models;

namespace ServerApp.Controllers
{
	[Route("/api/session")]
	[ApiController]
	public class SessionValuesController : Controller
	{
		const string cartKey = "cart";

		[HttpGet( cartKey )]
		public IActionResult GetCart()
		{
			return Ok( HttpContext.Session.GetString( cartKey ) );
		}

		[HttpPost( cartKey )]
		public void StoreCart( [FromBody] ProductSelection[] products )
		{
			string jsonData;
			jsonData = JsonConvert.SerializeObject( products );
			HttpContext.Session.SetString( cartKey, jsonData );
		}
	}
}
