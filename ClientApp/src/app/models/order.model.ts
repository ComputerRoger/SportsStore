import { Injectable, OnInit } from "@angular/core";
import { CartService } from "./cart.service";
import { Repository } from "./repository";
import { Router, NavigationStart } from "@angular/router";
import { filter } from "rxjs/operators";
import { Observable } from 'rxjs';

//	This class is the interface for persisting the progress of making an order to session.
type OrderSession =
	{
		name: string,
		address: string,
		cardNumber: string,
		cardExpiry: string,
		cardSecurityCode: string
	}

const checkoutKey = "checkout";

@Injectable()
export class Order implements OnInit
{
	orderId: number;
	name: string;
	address: string;
	payment: Payment = new Payment();

	submitted: boolean = false;
	shipped: boolean = false;
	orderConfirmation: OrderConfirmation;

	constructorCount = 0;
	routerEventCount = 0;
	sessionStoreCount = 0;

	constructor(
		private repository: Repository,
		public cartService: CartService,
		public router: Router)
	{
		this.constructorCount++;
		console.log('Order Constructor: ' + this.constructorCount);
		router.events
			.pipe(
				filter(event => event instanceof NavigationStart)
			)
			.subscribe(event =>
			{
				this.routerEventCount++;
				console.log('NavigationStart event received. ' + this.routerEventCount);
				if (router.url.startsWith("/" + checkoutKey)
					&& this.name != null
					&& this.address != null
				)
				{
					this.sessionStoreCount++;
					console.log('Would store session here. ' + this.sessionStoreCount);
					this.OrderToSession();
				}
			});

		//	Get the order data from session during construction.
		this.SessionToOrder();
	}

	ngOnInit()
	{
	}

	OrderToSession()
	{
		console.log('OrderToSession start.');
		this.repository.storeSessionData<OrderSession>(checkoutKey,
			{
				name: this.name,
				address: this.address,
				cardNumber: this.payment.cardNumber,
				cardExpiry: this.payment.cardExpiry,
				cardSecurityCode: this.payment.cardSecurityCode
			})
		console.log('OrderToSession done.');
	}

	SessionToOrder()
	{
		console.log('SessionToOrder start.');
		this.repository.getSessionData<OrderSession>(checkoutKey)
			.subscribe(data =>
			{
				if (data != null)
				{
					this.name = data.name;
					this.address = data.address;
					this.payment.cardNumber = data.cardNumber;
					this.payment.cardExpiry = data.cardExpiry;
					this.payment.cardSecurityCode = data.cardSecurityCode;
				}
			})
		console.log('SessionToOrder done.');
	}

	///////////////////		Properties		///////////////////

	get products(): CartLine[]
	{
		let cartLines = this.cartService.productSelections
			.map(productSelection => new CartLine(productSelection.productId, productSelection.quantity));
		return (cartLines);
	}

	clear()
	{
		this.name = null;
		this.address = null;
		this.payment = new Payment();
		this.cartService.clear();
		this.submitted = false;
	}

	submit()
	{
		this.submitted = true;
		this.repository.createOrder(this);
	}
}

export class Payment
{
	cardNumber: string;
	cardExpiry: string;
	cardSecurityCode: string;
}

export class CartLine
{
	constructor(
		private productId: number,
		private quantity: number)
	{
	}
}

export class OrderConfirmation
{
	constructor(public orderId: number,
		public authCode: string,
		public amount: number) { }
}