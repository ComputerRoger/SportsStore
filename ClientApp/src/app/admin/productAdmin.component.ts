import { Component, OnInit } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { Order } from "../models/order.model";

@Component({
	selector: 'productAdmin',
	templateUrl: 'productAdmin.component.html'
})
export class ProductAdminComponent implements OnInit
{
	constructor(private repository: Repository) { }

	ngOnInit(): void {

	}
	//////////////////////////	  Properties.		////////////////////

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

}
