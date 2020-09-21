import { Component } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { Router, ActivatedRoute } from "@angular/router";

@Component({
	selector: 'product-detail',
	templateUrl: './productDetail.component.html'
})
export class ProductDetailComponent {

	//Inject the model container.
	constructor(private repository: Repository,
		private router: Router,
		activatedRoute: ActivatedRoute) {
		let id = Number.parseInt(activatedRoute.snapshot.params["id"]);

		//	Navigate with the activated route using the id when it exists.
		if (id) {
			//	Get the data needed by the component.
			this.repository.getProduct(id);
		}
		else {
			//	Navigate to the default route.
			router.navigateByUrl("/");
		}
	}


	//////////////////////////	  Properties.		////////////////////
	get product(): Product {
		return this.repository.productData;
	}

	//////////////////////////	  Methods.		////////////////////


	///////////////////////////		Event handlers.		///////////////////

}
