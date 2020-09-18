
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
	public class SupplierData
	{
		[Required]
		public string Name { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		[StringLength( 2, MinimumLength =2)]
		public string State { get; set; }

		//	Provide a property that matches the database schema.
		public Supplier Supplier => new Supplier
		{
			Name = Name,
			City = City,
			State = State
		};
	}
}
