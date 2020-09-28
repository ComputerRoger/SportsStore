import { Component } from '@angular/core';
import { Repository } from "../models/repository";
import { NavigationService } from "../models/navigation.service";

import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";


@Component({
	selector: 'store-pagination',
	templateUrl: 'pagination.component.html'
})
export class PaginationComponent
{
	//Inject the model container.
	constructor(private repository: Repository, public navigationService: NavigationService ) { }

	//////////////////////////	  Properties.		////////////////////

	get pages(): number[] {
		if (this.navigationService.productCount > 0) {
			//	Create an array of page numbers.
			let pageCount = this.navigationService.productCount / this.navigationService.productsPerPage;
			let pageNumbers = Array(Math.ceil(pageCount)).fill(0).map((x, pageIndex) => pageIndex + 1);
			return pageNumbers;
		}
		else {
			return [];
		}
	}

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

}
