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
		public string NavigteToUrl( BrowserForm browserForm, string url )
		{
			BrowserForm.BrowserWorkDelegate browserWorkDelegate;
			browserWorkDelegate = new BrowserForm.BrowserWorkDelegate( browserForm.BrowserWork );
			object[] args = new object[ 1 ];
			args[ 0 ] = url;
			string result = ( string ) browserForm.Invoke( browserWorkDelegate, args );
			return ( result );
		}
		public string NavigateAndSync( BrowserForm browserForm, QueryBody postQueryBody )
		{
			string result = this.NavigteToUrl( browserForm, postQueryBody.ReceiveUrl );
			return ( result );
		}
		public string NavigateToBlank( BrowserForm browserForm )
		{
			string url = "https://www.blank.org/";
			string result = this.NavigteToUrl( browserForm, url );
			return ( result );
		}
		public ITcpFrame DecodeAndDelegate( ITcpFrame requestFrame, BrowserForm browserForm, AppDocument appDocument )
		{
			ByteArrayBase byteArrayBase = ( ByteArrayBase ) ByteArrayBase.ByteArrayToObject( requestFrame.FramePacket );
			ByteArrayBase.BrowserIpcEnum browserIpcEnum = byteArrayBase.BrowserIpc;
			RequestResponseFrame responseFrame;

			switch( browserIpcEnum )
			{
			case ByteArrayBase.BrowserIpcEnum.GetWebPage:
				GetWebPageIpc getWebPageIpc = ( GetWebPageIpc ) byteArrayBase;
				NavigateAndSync( browserForm, getWebPageIpc.PostQueryBody );
				string search = getWebPageIpc.PostQueryBody.ReceiveUrl;

				//	Wait for the results.
				BrowserDocument browserDocument = browserForm.BrowserDocument;

				//	Wait until all navigation results are completed.
				browserDocument.NavigateManualResetEvent.WaitOne();

				//	Get the results of navigation
				string domText = browserDocument.DomText;
				string xmlText = browserDocument.XmlText;


				//	HtmlDecode works.	Must be lowercase.
				string sampleHtml = "&lt;strong&gt;bold &LT/strong&GT;test";
				var resultHtml = System.Web.HttpUtility.HtmlDecode( sampleHtml );
				// result = "<strong>bold </strong>test"


				//	UrlDecode works.
				//string sampleUrl = "test %3cstrong%3ebold %3c/strong%3etest";
				//string sampleUrl = "test %3cstrong%3ebold %3C/strong%3Etest";
				//var resultUrl = System.Web.HttpUtility.UrlDecode( sampleUrl );
				// result = "<strong>bold </strong>test"


				//string sampleUrl = "test %3cstrong%3ebold %3c/strong%3etest";
				string sampleUrl = "https://images-na.ssl-images-amazon.com/images/I/41icwgAxVqL._RC|71RKo6SPKEL.css,21qFIynv1ZL.css,31FX6DlOvlL.css,21lRUdwotiL.css,41oKRlyPnmL.css,11G4HxMtMSL.css,31OvHRW XiL.css,01XHMOHpK1L.css_.css?AUIClients/NavDesktopUberAsset&amp;yKDHSa d#desktop.309131-T1";
				var resultUrl = System.Web.HttpUtility.UrlDecode( sampleUrl );

				List<string> textList = new List<string>();
				//textList.Add( "HTML " + sampleHtml + " " + resultHtml );
				//textList.Add( "URL " + sampleUrl + "\n\n\n" + resultUrl );
				textList.Add( domText );
				textList.Add( xmlText );
				textList.Add( browserDocument.DomContentOnly() );
				textList.Add( browserDocument.XmlContentOnly() );

				//	Frame the results.
				ResultQueryBody resultQueryBody = new ResultQueryBody( textList );
				ResultWebPageIpc resultWebPageIpc = new ResultWebPageIpc( resultQueryBody );
				responseFrame = new RequestResponseFrame( resultWebPageIpc.ToByteArray() );

				//	Navigate to a blank page so the pool of browsers remains quiet.
				NavigateToBlank( browserForm );
				break;
			case ByteArrayBase.BrowserIpcEnum.SIZE_BrowserIpcEnum:
			default:
				responseFrame = new RequestResponseFrame();
				break;
			}
			return ( responseFrame );
		}
		public ITcpFrame DoSomeWork( ITcpFrame requestFrame, AppDocument appDocument )
		{
			MainForm mainForm = appDocument.MainForm;

			//	Start the browser.
			MainForm.StartBrowserFormDelegate startBrowserFormDelegate;
			startBrowserFormDelegate = new MainForm.StartBrowserFormDelegate( mainForm.StartBrowserForm );
			BrowserForm browserForm = ( BrowserForm ) mainForm.Invoke( startBrowserFormDelegate );

			//	Decode the request and generate a response.
			ITcpFrame responseFrame = DecodeAndDelegate( requestFrame, browserForm, appDocument );

			//	Stop the browser.
			MainForm.StopBrowserFormDelegate stopBrowserFormDelegate;
			stopBrowserFormDelegate = new MainForm.StopBrowserFormDelegate( mainForm.StopBrowserForm );
			object[] args = new object[ 1 ];
			args[ 0 ] = browserForm;
			bool isRemoved = ( bool ) mainForm.Invoke( stopBrowserFormDelegate, args );

			//	Return the framed results.
			return responseFrame;
		}

		public ITcpFrame ServiceTheRequest( ITcpFrame requestFrame, ILogger logger, IAppDocument appDocument )
		{
			ITcpFrame tcpFrame;
			AppDocument theDocument = ( AppDocument ) appDocument;

			tcpFrame = DoSomeWork( requestFrame, theDocument );
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
