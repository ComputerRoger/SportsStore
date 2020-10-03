import { Component, OnInit } from '@angular/core';
import { Repository } from "../models/repository";
import { Product } from "../models/product.model";
import { Supplier } from "../models/supplier.model";
import { Order } from "../models/order.model";

@Component({
	selector: 'overview'
	, templateUrl: './overview.component.html'
	//,styleUrls: ['./overview.component.css']
})
export class OverviewComponent implements OnInit
{
	constructor(private repository: Repository) { }

	ngOnInit(): void
	{
	}

	//////////////////////////	  Properties.		////////////////////

	get products(): Product[] {
		return this.repository.products;
	}

	get orders(): Order[] {
		return this.repository.orders;
	}

	//////////////////////////	  Methods.		////////////////////

	///////////////////////////		Event handlers.		///////////////////

}
