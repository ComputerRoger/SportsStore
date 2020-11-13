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
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.CodeAnalysis.Operations;

namespace ServerApp.Controllers
{
	public class QueryBrowserController : ControllerBase
	{
		public string RemoteHostName { get; protected set; }
		public int RemotePortNumber { get; protected set; }
		public int SizeStreamBuffer { get; protected set; }

		public QueryBrowserController()
		{
			RemoteHostName = GeneralClassLibrary.Constants.BrowserServerHostName;
			RemotePortNumber = GeneralClassLibrary.Constants.BrowserServerPortNumber;
			SizeStreamBuffer = GeneralClassLibrary.Constants.SizeStreamBuffer;
		}

		//	Post-Redirect-Get pattern.
		[AllowAnonymous]        //	All access by any user.
		[HttpPost( "/api/QueryBrowser/Test" )]
		public async Task<IActionResult> TestSendReceive( [FromBody] XmlPageRequestBody xmlPageRequestBody )
		{
			ILogger logger = new ConsoleLogger();
			JsonResult jsonResult;
			RequestResponseFrame responseFrame;
			object jsonObject;

			string methodName = "TestSendReceive";

			logger.WriteEntry( methodName + " entry." );

			if( ModelState.IsValid )
			{
				logger.WriteEntry( methodName + " ModelState is valid." );

				string receiveUrl = xmlPageRequestBody.ReceiveUrl;
				string receiveSearch = xmlPageRequestBody.ReceiveSearch;
				
				GeneralClassLibrary.XmlPageRequestFrame xmlPageRequestFrame = new XmlPageRequestFrame( xmlPageRequestBody );

				//	Transform the API request to an ITcpFrame.
				byte[] sendFrameBytes = xmlPageRequestFrame.ToByteArray();
				RequestResponseFrame requestFrame = new RequestResponseFrame( sendFrameBytes );

				//	Connect to the server and obtain a network stream.
				BufferedStream bufferedStream = await SendReceiveReply.ConnectToServer( RemoteHostName, RemotePortNumber, SizeStreamBuffer, logger );

				//	Send and receive data via the stream.
				responseFrame = await SendReceiveReply.RequestReceiveResponse( bufferedStream, requestFrame, logger );
				ITcpFrame tcpFrame = responseFrame;
				byte[] replyFrameBytes = tcpFrame.FramePacket;

				IpcFrameBase ipcFrameBase = ( IpcFrameBase ) IpcFrameBase.FromByteArray( replyFrameBytes );

				//	Transform the ITcpFrame to an API response.
				logger.WriteEntry( methodName + " building the JSON result." );
				XmlPageReplyFrame xmlPageReplyFrame = ( XmlPageReplyFrame ) ipcFrameBase;
				jsonObject = new
				{
					textArray = xmlPageReplyFrame.ResultList,
					isSuccess = true
				};
			}
			else
			{
				logger.WriteEntry( methodName + " ModelState is not valid!" );
				jsonObject = new
				{
					textArray = new List<string>(),
					isSuccess = false
				};
			}

			logger.WriteEntry( methodName + " exit." );

			jsonResult = new JsonResult( jsonObject );
			return jsonResult;
		}

		[AllowAnonymous]        //	All access by any user.
		[HttpGet( "/api/QueryBrowser/Test" )]
		public IActionResult TestSendReceive( string responseText )
		{
			JsonResult jsonResult;
			ILogger logger = new ConsoleLogger();

			string methodName = "TestSendReceive";

			logger.WriteEntry( methodName + " entry." );

			if( responseText == null )
			{
				responseText = "Null responseText text.";
			}

			object jsonObject = new { reply = responseText };
			jsonResult = new JsonResult( jsonObject );

			return jsonResult;
		}
	}
}
