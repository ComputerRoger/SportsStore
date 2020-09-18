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
	public class ProductData
	{
		[Required]
		public string Name { 
			get => Product.Name; 
			set => Product.Name = value; 
		}

		[Required]
		public string Category { 
			get => Product.Category; 
			set => Product.Category = value; 
		}

		[Required]
		public string Description { 
			get => Product.Description; 
			set => Product.Description = value; 
		}

		[Range( 1, int.MaxValue, ErrorMessage ="Price must be at least 1")]
		public decimal Price { 
			get => Product.Price; 
			set => Product.Price = value; 
		}

		public long? Supplier { 
			get => Product.Supplier?.SupplierId ?? null;
			set {
				if( !value.HasValue )
				{
					Product.Supplier = null;
				}
				else
				{
					if( Product.Supplier == null )
					{
						Product.Supplier = new Supplier();
					}
					Product.Supplier.SupplierId = value.Value;
				}
			} 
		}

		//	Provide a property that matches the database schema.
		//	The Product property backing data is initialized with a default constructor.
		public Product Product { get; set; } = new Product();

		//public Product Product => new Product
		//{
		//	Name = Name,
		//	Category = Category,
		//	Description = Description,
		//	Price = Price,
		//	Supplier = Supplier == 0 ? null : new Supplier
		//	{
		//		//	The supplier object only has a SupplierId because the purpose
		//		//	of the POST operation is only to create a Product.
		//		SupplierId = Supplier
		//	}
		//};
	}
}
