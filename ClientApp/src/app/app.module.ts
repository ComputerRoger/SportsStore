import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';

import { ModelModule } from './models/model.module';
import { StoreModule } from './store/store.module';

import { AppComponent } from './app.component';



@NgModule({
	declarations: [
		AppComponent
	],
	imports: [
		BrowserModule,
		FormsModule,
		AppRoutingModule,
		ModelModule,
		StoreModule
	],
	providers: [],
	bootstrap: [AppComponent]
})
export class AppModule { }
