import { Component } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";

@Component({
	selector: 'store-products',
	templateUrl: 'productSelection.component.html'
})
export class ProductSelectionComponent
{
	//Inject the model container.
	constructor(private repository: Repository) { }

	//////////////////////////	  Properties.		////////////////////

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

	TestMeGet()
	{
		this.repository.queryBrowserGet();
	}
	TestMePost()
	{
		this.repository.queryBrowserPost();
	}
}
