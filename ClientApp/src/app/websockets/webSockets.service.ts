import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { Repository } from "../models/repository";

@Injectable()
export class WebSocketsService
{
	readonly webSocketsKey = "ws";

	/////////////////	Properties		//////////////////////


	constructor(private repository: Repository)
	{
	}


	/////////////////	Methods		//////////////////////

}
