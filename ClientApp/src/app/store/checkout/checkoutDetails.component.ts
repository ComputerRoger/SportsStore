import { Component, OnInit } from '@angular/core';
import { Router, Routes, RouterModule } from "@angular/router";
import { Order } from "../../models/order.model";
import { CartService } from '../../models/cart.service';

@Component({
	selector: 'checkoutDetails'
	, templateUrl: './checkoutDetails.component.html'
	//,styleUrls: ['./checkoutDetails.component.css']
})
export class CheckoutDetailsComponent implements OnInit
{
	constructor(private router: Router,
		public order: Order)
	{
		if (order.products.length == 0)
		{
			this.router.navigateByUrl("/cart");
		}
	}

	ngOnInit(): void
	{
	}
}
