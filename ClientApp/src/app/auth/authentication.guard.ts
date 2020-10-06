import { Injectable } from "@angular/core";
import { AuthenticationService } from "./authentication.service";
import { Observable, Subject, throwError, of } from "rxjs";
import { Router, ActivatedRouteSnapshot, RouterStateSnapshot, ActivatedRoute } from "@angular/router";
import { Repository } from "../models/repository";

@Injectable()
export class AuthenticationGuard
{
	constructor(private repository: Repository,
		private authenticationService: AuthenticationService,
		private router: Router) {
	}

	/////////////////	Properties		//////////////////////


	/////////////////	Methods		//////////////////////

	canActivateChild(route: ActivatedRouteSnapshot,
	state: RouterStateSnapshot): boolean
	{
		let isActivateChild;

		isActivateChild = this.authenticationService.isAuthenticated;
		if (isActivateChild)
		{
			//	The child route may be activated.
		}
		else
		{
			//	Apply the guard by navigating to a safe path.
			this.authenticationService.callbackUrl = route.url.toString();
			this.router.navigateByUrl("/admin/login");
		}
		return isActivateChild;
	}
}
