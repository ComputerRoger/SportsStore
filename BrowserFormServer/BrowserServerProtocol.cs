using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using GeneralClassLibrary;
using AsyncSockets;
using System.IO;
using System.IO.Ports;
using System.ComponentModel;
using System.Xml;

namespace BrowserFormServer
{
	public class BrowserServerProtocol : IServerProtocol
	{
		public BrowserServerProtocol( Socket clientSocket, ILogger logger, IServiceRequest serviceRequest )
		{
			ClientSocket = clientSocket;
			Logger = logger;
			TcpClient tcpClient = new TcpClient();
			TcpClient = tcpClient;
			TcpClient.Client = clientSocket;
			SizeStreamBuffer = 65535;
			ServiceRequest = serviceRequest;
		}

		public Socket ClientSocket { get; protected set; }
		public ILogger Logger { get; protected set; }
		public TcpClient TcpClient { get; protected set; }
		public int SizeStreamBuffer { get; protected set; }
		public IServiceRequest ServiceRequest { get; protected set; }

		//	Handle one connection cycle of Receive - Service - Reply.
		public async void HandleClientConnection( object appDocument )
		{
			BufferedStream bufferedStream;
			ILogger logger;
			IServiceRequest serviceRequest;
			TcpClient tcpClient;

			tcpClient = TcpClient;
			logger = Logger;
			serviceRequest = ServiceRequest;

			string methodName = "HandleClientConnection";

			logger.WriteEntry( methodName + " A connection has been made to the server." );

			NetworkStream networkStream = tcpClient.GetStream();
			bufferedStream = new BufferedStream( networkStream, SizeStreamBuffer );
			await SendReceiveReply.ReceiveServiceReply( bufferedStream, serviceRequest, logger, ( IAppDocument ) appDocument );
			bufferedStream.Close();
			logger.WriteEntry( methodName + " done" );
		}
	}

	public class BrowserServerProtocolFactory : IProtocolFactory
	{
		public BrowserServerProtocolFactory() { }
		public IServerProtocol CreateServerProtocol( Socket clientSocket, ILogger logger )
		{
			ServiceRequest serviceRequest;

			serviceRequest = new ServiceRequest();
			IServerProtocol serverProtocol = new BrowserServerProtocol( clientSocket, logger, serviceRequest );
			return ( serverProtocol );
		}
	}
}
