import { Component, OnInit } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { Order } from "../models/order.model";
import { AuthenticationService } from "../auth/authentication.service";


@Component({
	selector: 'admin-component',
	templateUrl: 'admin.component.html'
})
export class AdminComponent implements OnInit {
	constructor(private repository: Repository, public authenticationService : AuthenticationService) {
		repository.filter.reset();
		repository.filter.related = true;
		repository.getProducts();
		repository.getSuppliers();
		repository.getOrders();
	}

	ngOnInit(): void {

	}

	//////////////////////////	  Properties.		////////////////////

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

}
