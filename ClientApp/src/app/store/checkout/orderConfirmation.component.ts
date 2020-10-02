import { Component, OnInit } from '@angular/core';
import { Router, Routes, RouterModule } from "@angular/router";
import { Order } from "../../models/order.model";
import { CartService } from '../../models/cart.service';

@Component({
	selector: 'orderConfirmation'
	, templateUrl: './orderConfirmation.component.html'
	//,styleUrls: ['./orderConfirmation.component.css']
})
export class OrderConfirmationComponent implements OnInit
{
	constructor(private router: Router,
		public order: Order)
	{
		if (!order.submitted)
		{
			this.router.navigateByUrl("/checkout/step3");
		}
	}

	ngOnInit(): void
	{
	}
}
