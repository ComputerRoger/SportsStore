import { Component } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { CartService } from "../models/cart.service";

@Component({
	selector: 'cart-summary',
	templateUrl: 'cartSummary.component.html'
})
export class CartSummaryComponent
{
	//Inject the model container.
	constructor(private repository: Repository, private cartService: CartService) { }

	//////////////////////////	  Properties.		////////////////////

	get itemCount(): number
	{
		return this.cartService.itemCount;
	}

	get totalPrice(): number
	{
		return (this.cartService.totalPrice);
	}

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

}
