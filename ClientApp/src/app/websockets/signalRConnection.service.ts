import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Repository } from "../models/repository";

@Injectable()
export class SignalRConnectionService
{
	readonly chatHubKey = "/chat";
	messageCounter;

	/////////////////	Properties		//////////////////////


	constructor(private repository: Repository)
	{
		this.messageCounter = 0;
	}


	/////////////////	Methods		//////////////////////

	//	Factory

	createObservableService(): Observable<string>
	{
		let observableService = <Observable<string>>new Observable(
			observer =>
			{
				setInterval(() => observer.next("NewMessage " + this.messageCounter++), 1000);
			});
		return observableService;
	}
}
