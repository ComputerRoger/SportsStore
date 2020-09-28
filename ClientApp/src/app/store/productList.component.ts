import { Component } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { CartService } from "../models/cart.service";

@Component({
	selector: 'store-product-list',
	templateUrl: 'productList.component.html'
})
export class ProductListComponent
{
	//Inject the model container.
	constructor(private repository: Repository, private cartService: CartService) { }

	//////////////////////////	  Properties.		////////////////////

	//	Get the list of products filtered by pagination.
	get products(): Product[]
	{
		if (this.repository.products != null &&
			this.repository.products.length > 0)
		{
			let pageIndex = (this.repository.pagination.currentPage - 1) *
				this.repository.pagination.productsPerPage;

			//	Select a subset of the products available.
			return this.repository.products.slice(pageIndex,
				pageIndex + this.repository.pagination.productsPerPage);
		}
		else
		{
			//	Do nothing.
		}
	}

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

	addToCart(product: Product)
	{
		this.cartService.addProduct(product);
	}

}
