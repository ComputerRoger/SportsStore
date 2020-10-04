import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';

import { ModelModule } from './models/model.module';
import { StoreModule } from './store/store.module';

import { AppComponent } from './app.component';

import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { ErrorHandlerService } from "./errorHandler.service";


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
	providers: [
		ErrorHandlerService,
		{
			provide: HTTP_INTERCEPTORS,
			useExisting: ErrorHandlerService,
			multi: true
		}
	],
	bootstrap: [AppComponent]
})
export class AppModule { }
