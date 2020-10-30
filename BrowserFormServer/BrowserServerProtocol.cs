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

		//	Do application specific work here.
		//	It could be a looped protocol exchanging a series of instructions.
		//	Or it could be a simple receive and reply.
		//	The client should close the socket.
		//	However, in HTTP, it is the server that closes the connection.
		//	That is why HTTP is a connectionless protocol.
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

	public class ServiceRequest : IServiceRequest
	{
		public string NavigateAndSync( BrowserForm browserForm, AppDocument appDocument )
		{
			BrowserForm.BrowserWorkDelegate browserWorkDelegate;
			browserWorkDelegate = new BrowserForm.BrowserWorkDelegate( browserForm.BrowserWork );
			object[] args = new object[ 1 ];
			args[ 0 ] = "https://amazon.com";
			string result = ( string ) browserForm.Invoke( browserWorkDelegate, args );
			return ( result );
		}
		public async Task<RequestResponseFrame> DoSomeWork( ITcpFrame requestFrame, AppDocument appDocument )
		{
			MainForm mainForm = appDocument.MainForm;

			//	Start the browser.
			MainForm.StartBrowserFormDelegate startBrowserFormDelegate;
			startBrowserFormDelegate = new MainForm.StartBrowserFormDelegate( mainForm.StartBrowserForm );
			BrowserForm browserForm = ( BrowserForm ) mainForm.Invoke( startBrowserFormDelegate );

			//	Do some more work.
			string result = NavigateAndSync( browserForm, appDocument );
			BrowserDocument browserDocument = browserForm.BrowserDocument;

			//	Wait until all navigation results are completed.
			browserDocument.NavigateManualResetEvent.WaitOne();
			result = browserDocument.DomText;

			//	Generate a response to the client.
			RequestResponseFrame requestResponseFrame = new RequestResponseFrame( requestFrame.FramePacket );

			//	Stop the browser.
			MainForm.StopBrowserFormDelegate stopBrowserFormDelegate;
			stopBrowserFormDelegate = new MainForm.StopBrowserFormDelegate( mainForm.StopBrowserForm );
			object[] args = new object[ 1 ];
			args[ 0 ] = browserForm;
			bool isRemoved = ( bool ) mainForm.Invoke( stopBrowserFormDelegate, args );
			return requestResponseFrame;
		}

		public async Task<ITcpFrame> ServiceTheRequest( ITcpFrame requestFrame, ILogger logger, IAppDocument appDocument )
		{
			ITcpFrame tcpFrame;
			AppDocument theDocument = ( AppDocument ) appDocument;

			tcpFrame = await DoSomeWork( requestFrame, theDocument );
			return tcpFrame;
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
