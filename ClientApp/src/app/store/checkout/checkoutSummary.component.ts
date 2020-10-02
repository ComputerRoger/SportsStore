import { Component, OnInit } from '@angular/core';
import { Router, Routes, RouterModule } from "@angular/router";
import { Order } from "../../models/order.model";
import { CartService } from '../../models/cart.service';

@Component({
	selector: 'checkoutSummary'
	, templateUrl: './checkoutSummary.component.html'
	//,styleUrls: ['./checkoutSummary.component.css']
})
export class CheckoutSummaryComponent implements OnInit
{
	constructor(private router: Router,
		public order: Order)
	{
		if (order.payment.cardNumber == null
			|| order.payment.cardExpiry == null
			|| order.payment.cardSecurityCode == null)
		{
			this.router.navigateByUrl("/checkout/step2");
		}
	}

	ngOnInit(): void
	{
	}

	submitOrder()
	{
		this.order.submit();
		this.router.navigateByUrl("/checkout/confirmation");
	}
}
