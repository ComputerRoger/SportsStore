import { Injectable } from "@angular/core";
import { HttpEvent, HttpInterceptor, HttpRequest, HttpErrorResponse, HttpHandler } from "@angular/common/http";
import { Observable, Subject, throwError } from "rxjs";
import { catchError } from "rxjs/operators";

@Injectable()
export class ErrorHandlerService implements HttpInterceptor
{
	private errSubject = new Subject<string[]>();

	/////////////////	Properties		//////////////////////

	get errors(): Observable<string[]>
	{
		return this.errSubject;
	}

	/////////////////	Methods		//////////////////////

	intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>>
	{
		return next.handle(request).pipe(
			catchError((errResponse: HttpErrorResponse) =>
			{
				if (errResponse.error.errors)
				{
					this.errSubject.next([...Object.values(errResponse.error.errors) as string[]]);
				}
				else if (errResponse.error.title)
				{
					this.errSubject.next([errResponse.error.title]);
				}
				else
				{
					this.errSubject.next(["An HTTP error occurred."]);
				}
				return throwError(errResponse);
			})
		);
	}
}
