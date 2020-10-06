import { Component } from '@angular/core';
import { Repository } from "../models/repository";
import { AuthenticationService } from "./authentication.service";

@Component({
	selector: 'authentication',
	templateUrl: 'authentication.component.html'
})
export class AuthenticationComponent
{
	isShowError: boolean = false;

	constructor(private repository: Repository, public authenticationService: AuthenticationService) { }

	//////////////////////////	  Properties.		////////////////////

	//////////////////////////	  Methods.		////////////////////

	login() {
		this.isShowError = false;
		this.authenticationService.login().subscribe(
			result => this.isShowError = !result )
	}

	///////////////////////////		Event handlers.		///////////////////

}
