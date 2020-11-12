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
			RemoteHostName = "127.0.0.1";
			RemotePortNumber = 50500;
			SizeStreamBuffer = 65535;
		}


		//	Post-Redirect-Get pattern.
		[AllowAnonymous]        //	All access by any user.
		[HttpPost("/api/QueryBrowser/Test")]
		public async Task<IActionResult> TestSendReceive([FromBody] QueryBody postQueryBody)
		{
			ILogger logger = new ConsoleLogger();
			JsonResult jsonResult;
			ResultQueryBody resultQueryBody;
			RequestResponseFrame responseFrame;

			string methodName = "TestSendReceive";

			logger.WriteEntry(methodName + " entry.");

			if (ModelState.IsValid)
			{
				logger.WriteEntry(methodName + " ModelState is valid.");
				string receiveUrl = postQueryBody.ReceiveUrl;
				string receiveSearch = postQueryBody.ReceiveSearch;

				if (receiveUrl is null)
				{
					logger.WriteEntry(methodName + " null receiveUrl.");
				}
				else
				{
					logger.WriteEntry(methodName + " receiveUrl = " + receiveUrl);
				}
				if (receiveSearch is null)
				{
					logger.WriteEntry(methodName + " null receiveSearch.");
				}
				else
				{
					logger.WriteEntry(methodName + " receiveSearch = " + receiveSearch);
				}
			}
			else
			{
				logger.WriteEntry(methodName + " ModelState is not valid!");
			}

			GeneralClassLibrary.GetWebPageIpc getWebPageIpc = new GetWebPageIpc(postQueryBody);

			//	Transform the API request to an ITcpFrame.
			byte[] sendFrameBytes = getWebPageIpc.ToByteArray();
			RequestResponseFrame requestFrame = new RequestResponseFrame(sendFrameBytes);

			//	Connect to the server and obtain a network stream.
			BufferedStream bufferedStream = await SendReceiveReply.ConnectToServer(RemoteHostName, RemotePortNumber, SizeStreamBuffer, logger);

			//	Send and receive data via the stream.
			responseFrame = await SendReceiveReply.RequestReceiveResponse(bufferedStream, requestFrame, logger);
			ITcpFrame tcpFrame = responseFrame;
			byte[] replyFrameBytes = tcpFrame.FramePacket;

			ByteArrayBase byteArrayBase = ResultWebPageIpc.FromByteArray(replyFrameBytes);

			//	Transform the ITcpFrame to an API response.
			logger.WriteEntry(methodName + " building the JSON result. ");
			object jsonObject;
			switch (byteArrayBase.BrowserIpc)
			{
				case ByteArrayBase.BrowserIpcEnum.ResultWebPage:
					ResultWebPageIpc resultWebPageIpc = (ResultWebPageIpc)byteArrayBase;
					resultQueryBody = resultWebPageIpc.ResultQueryBody;
					jsonObject = new
					{
						textArray = resultQueryBody.ResultList,
						isSuccess = true
					};
					break;
				case ByteArrayBase.BrowserIpcEnum.GetWebPage:
				case ByteArrayBase.BrowserIpcEnum.SIZE_BrowserIpcEnum:
				default:
					jsonObject = new { };
					break;
			}
			logger.WriteEntry(methodName + " exit.");

			jsonResult = new JsonResult(jsonObject);
			return jsonResult;
			//return Ok();
		}

		[AllowAnonymous]        //	All access by any user.
		[HttpGet("/api/QueryBrowser/Test")]
		public IActionResult TestSendReceive(string responseText)
		{
			JsonResult jsonResult;
			ILogger logger = new ConsoleLogger();

			string methodName = "TestSendReceive";

			logger.WriteEntry(methodName + " entry.");

			if (responseText == null)
			{
				responseText = "Null responseText text.";
			}

			object jsonObject = new { reply = responseText };
			jsonResult = new JsonResult(jsonObject);

			return jsonResult;

			//	Moved to POST.

			// RequestResponseFrame responseFrame;
			// //	Transform the API request to an ITcpFrame.
			// byte[] sendFrameBytes = new byte[ 0 ];
			// RequestResponseFrame requestFrame = new RequestResponseFrame( sendFrameBytes );

			// //	A network stream is used to send and receive data.
			// BufferedStream bufferedStream = await SendReceiveReply.ConnectToServer( RemoteHostName, RemotePortNumber, SizeStreamBuffer, logger );
			// responseFrame = await SendReceiveReply.RequestReceiveResponse( bufferedStream, requestFrame, logger );

			// //	Transform the ITcpFrame to an API response.
			// logger.WriteEntry( methodName + " receive buffer stream closed." );
		}
	}
}
