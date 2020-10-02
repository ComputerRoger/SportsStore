import { Component, OnInit } from '@angular/core';
import { Router, Routes, RouterModule } from "@angular/router";
import { Order } from "../../models/order.model";
import { CartService } from '../../models/cart.service';

@Component({
	selector: 'checkoutPayment'
	, templateUrl: './checkoutPayment.component.html'
	//,styleUrls: ['./checkoutPayment.component.css']
})
export class CheckoutPaymentComponent implements OnInit
{
	constructor(private router: Router,
		public order: Order)
	{
		if (order.name == null || order.address == null)
		{
			this.router.navigateByUrl("/checkout/step1");
		}
	}

	ngOnInit(): void
	{
	}
}
