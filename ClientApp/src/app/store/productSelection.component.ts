import { Component } from '@angular/core';
import { Repository, PostQueryBody, QueryBrowserResult } from "../models/repository";
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
		let postQueryBody = new PostQueryBody("https://google.com", "C++");
		let queryBrowserResult = this.repository.queryBrowserPost(postQueryBody);
	}
}
