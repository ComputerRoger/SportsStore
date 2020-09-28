import { Component, OnInit } from '@angular/core';
import { Routes, RouterModule } from "@angular/router";
import { ProductSelectionComponent } from "./productSelection.component";
import { CartService } from '../models/cart.service';
import { ProductSelection } from "../models/ProductSelection";

@Component({
	selector: 'app-cart-detail'
	, templateUrl: './cart-detail.component.html'
	//,styleUrls: ['./cart-detail.component.css']
})
export class CartDetailComponent implements OnInit
{
	constructor(public cartService: CartService) { }

	ngOnInit(): void
	{
	}
}
