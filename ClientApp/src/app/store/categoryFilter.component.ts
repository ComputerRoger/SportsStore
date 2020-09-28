import { Component } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { NavigationService } from "../models/navigation.service";

@Component({
	selector: 'store-category-filter',
	templateUrl: 'categoryFilter.component.html'
})
export class CategoryFilterComponent
{
	//Inject the model container.
	constructor(public navigationService: NavigationService) { }

	//////////////////////////	  Properties.		////////////////////

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

}
