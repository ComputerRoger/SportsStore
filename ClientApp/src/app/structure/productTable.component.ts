import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";

@Component({
	selector: 'product-table',
	templateUrl: './productTable.component.html'
})
export class ProductTableComponent {

	//Inject the model container.
	constructor(private repository: Repository, private router: Router) { }

	//////////////////////////	  Properties.		////////////////////

	get products(): Product[] {
		return this.repository.products;
	}

	///////////////////////////		Event handlers		///////////////////

	//	Angular provides 2 means of navigation:
	//	A.	In the component: use router.navigateByUrl
	//	B.	In the template: use element attribute routerLink="SomeRoutePath"

	//	The component navigates.
	selectProduct(id: number) {
		//	Update the repository.
		this.repository.getProduct(id);
		//	Navigate to the detail component to show the new state.
		this.router.navigateByUrl("/detail/"+id);
	}
}
