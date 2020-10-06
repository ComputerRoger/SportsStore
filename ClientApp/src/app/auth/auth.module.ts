import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { Routes, RouterModule } from "@angular/router";

import { AuthenticationComponent } from './authentication.component';

import { CommonModule } from '@angular/common';
import { AuthenticationService } from './authentication.service';
import { AuthenticationGuard } from './authentication.guard';

@NgModule({
	declarations: [
		AuthenticationComponent
	],
	imports: [
		CommonModule,
		FormsModule,
		RouterModule
	],
	providers: [AuthenticationService, AuthenticationGuard],
	exports: [AuthenticationComponent]
})
export class AuthModule { }
