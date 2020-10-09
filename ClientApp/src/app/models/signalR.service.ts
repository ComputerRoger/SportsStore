import { Injectable } from "@angular/core";

//	npm install @aspnet/signalr --save --force

//import * as signalR from '@aspnet/signalr';
import { HubConnectionBuilder, LogLevel, HubConnection, HubConnectionState } from '@aspnet/signalr';

//	The following must match Startup.cs - Configure().
//	endpoints.MapHub<ChatHub>("/chat");
const signalRHubUrl = "/chat";

//	The following must match the interface class defined in the server.
const signalRHubMethodName = "SendMessage";
const signalRClientMethodName = "newMessage";

@Injectable()
export class SignalRService
{

	hubConnection: HubConnection;
	messageSender = "";
	messageText = "";
	isVerbose = true;

	constructor()
	{
		//	Enable WebSocket communication using a SignalR Hub.
		this.initializeSignalR();
	}

	initializeSignalR()
	{

		//	Startup.cs in the server has this endpoint:
		//	endpoints.MapHub<ChatHub>("/chat");

		//	Use the factory to create a HubConnection.
		this.hubConnection = new HubConnectionBuilder()
			.configureLogging(LogLevel.Information)
			.withUrl(signalRHubUrl)
			.build();

		//	Start the hubConnection before using it.
		this.hubConnection.start().then(() =>
		{
			//	Log the message received to the console.
			this.beVerbose("The hub connection has started.");
			this.hubConnection.on(signalRClientMethodName, (senderParameter, messageParameter) =>
			{
				this.messageSender = senderParameter;
				this.messageText = messageParameter;
				this.beVerbose("Success!  Message received from the hub! " + `${senderParameter}:${messageParameter}`);
			});
		});
	}

	public broadcastMessage(message: string)
	{
		if (this.hubConnection)
		{
			this.beVerbose("signalR hubConnection is defined.");
			if (this.hubConnection.state == HubConnectionState.Connected)
			{
				this.beVerbose("signalR hubConnection is Connected.");

				this.hubConnection.invoke(signalRHubMethodName, "This is an invoked message via hub method: " + signalRHubMethodName);
				this.beVerbose("done broadcasting: " + message);
				this.hubConnection.send(signalRHubMethodName, "Using send() with " + signalRHubMethodName);
			}
			else if (this.hubConnection.state == HubConnectionState.Disconnected)
			{
				this.beVerbose("signalR hubConnection is Disconnected.");
			}
			else
			{
				this.beVerbose("signalR hubConnection state is not known.");
			}
		}
		else
		{
			this.beVerbose("signalR hubConnection is null.");
		}
	}

	//	End of the class.
	private beVerbose(text: string)
	{
		if (this.isVerbose)
		{
			console.log(text);
		}
	}
}
