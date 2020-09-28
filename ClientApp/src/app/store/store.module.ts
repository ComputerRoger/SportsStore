import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from "@angular/router";

import { CartSummaryComponent } from './cartSummary.component';
import { CategoryFilterComponent } from './categoryFilter.component';
import { PaginationComponent } from './pagination.component';
import { ProductListComponent } from './productList.component';
import { RatingsComponent } from './ratings.component';
import { ProductSelectionComponent } from './productSelection.component';
import { CartDetailComponent } from './cart-detail.component';

@NgModule({
	declarations: [
		CartSummaryComponent,
		CategoryFilterComponent,
		PaginationComponent,
		ProductListComponent,
		RatingsComponent,
		ProductSelectionComponent,
		CartDetailComponent
	],
	imports: [
		BrowserModule,
		FormsModule,
		RouterModule
	],
	providers: [],
	bootstrap: [ProductSelectionComponent]
})
export class StoreModule { }
