import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ModelModule } from './models/model.module';

import { CategoryFilterComponent } from './structure/categoryFilter.component';
import { ProductTableComponent } from './structure/productTable.component';
import { ProductDetailComponent } from './structure/productDetail.component';

@NgModule({
	declarations: [
		AppComponent,
		CategoryFilterComponent,
		ProductTableComponent,
		ProductDetailComponent
	],
	imports: [
		BrowserModule,
		AppRoutingModule,
		ModelModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule { }
