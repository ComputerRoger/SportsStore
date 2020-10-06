import { Injectable } from "@angular/core";
import { HttpEvent, HttpInterceptor, HttpRequest, HttpErrorResponse, HttpHandler } from "@angular/common/http";
import { Observable, Subject, throwError, of } from "rxjs";
import { Router } from "@angular/router";
import { map, catchError } from 'rxjs/operators';
import { Repository } from "../models/repository";

@Injectable()
export class AuthenticationService
{
	constructor(private repository: Repository,
		private router: Router) {
	}

	isAuthenticated: boolean = false;
	name: string;
	password: string;
	callbackUrl: string;

	/////////////////	Properties		//////////////////////


	/////////////////	Methods		//////////////////////

	login(): Observable<boolean>
	{
		this.isAuthenticated = false;

		return this.repository.login(this.name, this.password)
			.pipe(map(response => {
				if (response) {
					this.isAuthenticated = true;
					this.password = null;
					this.router.navigateByUrl(this.callbackUrl || "/admin/overview");
				}
				return this.isAuthenticated;
			}),
				catchError(e => {
					this.isAuthenticated = false;
					return of(false);
				}));
	}

	logout()
	{
		this.isAuthenticated = false;
		this.repository.logout();
		this.router.navigateByUrl("/admin/login");
	}
}
