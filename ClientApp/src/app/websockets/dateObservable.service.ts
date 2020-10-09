import { Injectable } from "@angular/core";
import { Repository } from "../models/repository";
import { Observable } from 'rxjs';

@Injectable()
export class DateObservableService
{

	/////////////////	Properties		//////////////////////


	constructor(private repository: Repository)
	{
	}


	/////////////////	Methods		//////////////////////

	//	Factory

	createObservableService(): Observable<Date> {
		let observableService = <Observable < Date >> new Observable(
			observer => {
				setInterval(() => observer.next(new Date()), 1000);
			});
		return observableService;
	}
}
