import { Component } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";

@Component({
	selector: 'category-filter',
	templateUrl: './categoryFilter.component.html'
})
export class CategoryFilterComponent {
	public chessCategory = "chess";

	//Inject the model container.
	constructor(private repository: Repository) { }

	//////////////////////////	  Properties.		////////////////////

	//////////////////////////	  Methods.		////////////////////

	setCategory(category: string) {
		//	Configure the repository filter.
		this.repository.filter.category = category;
		//	Refresh the application data.
		this.repository.getProducts();
	}

	///////////////////////////		Event handlers to initiate POSTs.		///////////////////
	///////////////////////////		Event handlers to initiate PUTs.		///////////////////
	///////////////////////////		Event handlers to initiate PATCHs.		///////////////////
	///////////////////////////		Event handlers to initiate DELETEs.		///////////////////
}
