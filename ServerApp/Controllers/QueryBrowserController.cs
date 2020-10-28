using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AsyncSockets;
using GeneralClassLibrary;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace ServerApp.Controllers
{
	public class QueryBrowserController : ControllerBase
	{
		public string RemoteHostName { get; protected set; }
		public int RemotePortNumber { get; protected set; }
		public int SizeStreamBuffer { get; protected set; }

		public QueryBrowserController()
		{
			RemoteHostName = "127.0.0.1";
			RemotePortNumber = 50500;
			SizeStreamBuffer = 65535;
		}

		//[AllowAnonymous]        //	All access by any user.
		//[HttpPost("/api/QueryBrowser/Test")]
		//public IActionResult TestPost()
		//{
		//	//	Connect to the server.
		//	string serverName = ServerName;
		//	int serverPort = ServerPort;
		//	ILogger logger = new ConsoleLogger();
		//	AsyncSockets.AsyncConnect asyncConnect = new AsyncConnect(serverName, serverPort, logger);
		//	asyncConnect.Connect();

		//	//	Send a message to the server.
		//	Socket connectedSocket = asyncConnect.ClientSocket;
		//	logger.WriteEntry("This is a test log message in the API server.");
		//	AsyncSend asyncSend = new AsyncSend(connectedSocket, logger);
		//	asyncSend.Send( "Hello from QueryBrowser" + "\n" );
		//	System.Threading.Thread.Sleep(3000);
		//	asyncSend.Send( " Socket LocalEndPoint: " + connectedSocket.LocalEndPoint.ToString() + "\n" );
		//	System.Threading.Thread.Sleep( 1000 );
		//	asyncSend.Send( " Socket RemoteEndPoint: " + connectedSocket.RemoteEndPoint.ToString() + "\n" );
		//	System.Threading.Thread.Sleep( 1000 );
		//	connectedSocket.Shutdown(SocketShutdown.Send);

		//	//	Receive a response from the server.
		//	AsyncReceive asyncReceive = new AsyncReceive( connectedSocket, logger );
		//	string receiveText = asyncReceive.Receive();
		//	if( receiveText.Length > 0 )
		//	{
		//		logger.WriteEntry( "TestPost: received a reply." );
		//		logger.WriteEntry( receiveText );
		//	}
		//	else
		//	{
		//		logger.WriteEntry( "TestPost: received ZERO length reply." );
		//	}
		//	return Ok();
		//}

		[AllowAnonymous]        //	All access by any user.
		[HttpGet("/api/QueryBrowser/Test")]
		public async Task<IActionResult> TestSendReceive()
		{
			RequestResponseFrame responseFrame;
			ILogger logger = new ConsoleLogger();

			string methodName = "TestSendReceive";

			logger.WriteEntry( methodName + " entry." );

			//	Transform the API request to an ITcpFrame.
			byte[] sendFrameBytes = new byte[ 0 ];
			RequestResponseFrame requestFrame = new RequestResponseFrame( sendFrameBytes );

			//	A network stream is used to send and receive data.
			BufferedStream bufferedStream = await SendReceiveReply.ConnectToServer( RemoteHostName, RemotePortNumber, SizeStreamBuffer, logger );
			responseFrame = await SendReceiveReply.RequestReceiveResponse( bufferedStream, requestFrame, logger );

			//	Transform the ITcpFrame to an API response.
			logger.WriteEntry( methodName + " receive buffer stream closed." );
			return Ok();
		}
	}
}
