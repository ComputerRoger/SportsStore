using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace ServerApp.Models
{
	public class ChatHub : Hub
	{
		public const string clientMethodName = "newMessage";
		public const string anonymousUserName = "anonymous";
		public async Task SendMessage( string message )
		{
			await Clients.All.SendAsync( clientMethodName, anonymousUserName, message );
		}
	}
}
