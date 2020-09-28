import { CartService } from './cart.service';

//	This class is per line item of the cart.
//	There will be a corresponding class in ASP.Net.

export class ProductSelection
{
	public constructor(public cartService: CartService,
		public productId?: number,
		public name?: string,
		public price?: number,
		private m_quantity?: number) { }

	/////////////////	Properties must match Session interface.		//////////////////////
	public get quantity()
	{
		return (this.m_quantity);
	}
	public set quantity(newQuantity: number)
	{
		this.m_quantity = newQuantity;
		this.cartService.update();
	}
}
