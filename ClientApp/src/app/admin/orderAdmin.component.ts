import { Component, OnInit } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { Order } from "../models/order.model";

@Component({
	selector: 'orderAdmin',
	templateUrl: 'orderAdmin.component.html'
})
export class OrderAdminComponent implements OnInit
{
	constructor(private repository: Repository) { }

	ngOnInit(): void {

	}

	//////////////////////////	  Properties.		////////////////////

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

}
