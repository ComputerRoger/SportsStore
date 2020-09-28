namespace ServerApp.Models
{
	public class ProductSelection
	{
		//	The public properties must match the TypeScript public property names.
		public long productId { get; set; }
		public string name { get; set; }
		public decimal price { get; set; }
		public int quantity { get; set; }
	}
}

/**
//	This is the corresponding TypeScript class:

export class ProductSelection
{
	constructor(public cartService: CartService,
		public productId?: number,
		public name?: string,
		public price?: number,
		private m_quantity?: number)
	{ }

	/////////////////	Properties		//////////////////////

	get quantity()
	{
		return (this.m_quantity);
	}

	set quantity(quantity: number)
	{
		this.m_quantity = quantity;
		this.cartService.update();
	}
}
**/