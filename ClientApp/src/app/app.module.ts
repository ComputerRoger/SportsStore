import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
//import { FormsModule } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { Routes, RouterModule } from '@angular/router';

import { AppRoutingModule } from './app-routing.module';

import { ModelModule } from './models/model.module';
import { StoreModule } from './store/store.module';
import { AngularMaterialModule } from './angular-material/angular-material.module';

import { AppComponent } from './app.component';

import { HTTP_INTERCEPTORS } from "@angular/common/http";
import { ErrorHandlerService } from "./errorHandler.service";

import { TextObservableService } from "./websockets/textObservable.service";
import { DateObservableService } from "./websockets/dateObservable.service";
import { DevelopComponent } from './develop/develop.component';

@NgModule({
	declarations: [
		AppComponent,
		DevelopComponent
	]
	, imports: [
		BrowserModule,
		// FormsModule,
		ReactiveFormsModule,
		AppRoutingModule,
		ModelModule,
		StoreModule,
		AngularMaterialModule
	]
	, providers: [
		ErrorHandlerService
		, {
			provide: HTTP_INTERCEPTORS,
			useExisting: ErrorHandlerService,
			multi: true
		}
		, TextObservableService
		, DateObservableService
	]
	, bootstrap: [AppComponent]
})
export class AppModule { }
