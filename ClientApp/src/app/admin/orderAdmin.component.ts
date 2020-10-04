import { Component, OnInit } from '@angular/core';
import { Repository } from "../models/repository";
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

	get orders(): Order[] {
		return this.repository.orders;
	}

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

	markShipped(order: Order) {
		this.repository.shipOrder(order);
	}
}
