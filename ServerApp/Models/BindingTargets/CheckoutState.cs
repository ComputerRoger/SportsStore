using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace ServerApp.Models.BindingTargets
{

	//	Define a data-binding class that validates before EntityFramework receipt.
	//	The properties match the JSON model posted by the client, so data-binding is simple.
	//	Attributes do basic validation.
	//	Application specific validation should also be done.
	public class CheckoutState
	{
		public string name { get; set; }
		public string address { get; set; }
		public string cardNumber { get; set; }
		public string cardExpiry { get; set; }
		public string cardSecurityCode { get; set; }

	}
}
