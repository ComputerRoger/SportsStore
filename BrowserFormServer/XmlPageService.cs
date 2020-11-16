using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralClassLibrary;
using AsyncSockets;

namespace BrowserFormServer
{
	public class XmlPageService : IPerformService
	{
		public XmlPageService( XmlPageRequestFrame xmlPageRequestFrame ) 
		{
			XmlPageRequestFrame = xmlPageRequestFrame;
		}

		public XmlPageRequestFrame XmlPageRequestFrame { get; protected set; }
		public ITcpFrame PerformService( ILogger logger, IAppDocument iAppDocument )
		{
			AppDocument appDocument;

			appDocument = ( AppDocument ) iAppDocument;
			MainForm mainForm = appDocument.MainForm;

			//	Start the browser.
			MainForm.StartBrowserFormDelegate startBrowserFormDelegate;
			startBrowserFormDelegate = new MainForm.StartBrowserFormDelegate( mainForm.StartBrowserForm );
			BrowserForm browserForm = ( BrowserForm ) mainForm.Invoke( startBrowserFormDelegate );

			//	Decode the request and generate a response.
			ITcpFrame responseFrame = BuildResponse( browserForm );

			//	Stop the browser.
			MainForm.StopBrowserFormDelegate stopBrowserFormDelegate;
			stopBrowserFormDelegate = new MainForm.StopBrowserFormDelegate( mainForm.StopBrowserForm );
			object[] args = new object[ 1 ];
			args[ 0 ] = browserForm;
			bool isRemoved = ( bool ) mainForm.Invoke( stopBrowserFormDelegate, args );

			//	Return the framed results.
			return responseFrame;
		}
		public string NavigteToUrl( BrowserForm browserForm, string url )
		{
			BrowserForm.BrowserWorkDelegate browserWorkDelegate;
			browserWorkDelegate = new BrowserForm.BrowserWorkDelegate( browserForm.BrowserWork );
			object[] args = new object[ 1 ];
			args[ 0 ] = url;
			string result = ( string ) browserForm.Invoke( browserWorkDelegate, args );
			return ( result );
		}
		public string NavigateAndSync( BrowserForm browserForm, XmlPageRequestBody xmlPageRequestBody )
		{
			string result = this.NavigteToUrl( browserForm, xmlPageRequestBody.ReceiveUrl );
			return ( result );
		}
		public string NavigateToBlank( BrowserForm browserForm )
		{
			string url = GeneralClassLibrary.Constants.BlankPageSite;
			string result = this.NavigteToUrl( browserForm, url );
			return ( result );
		}
		public ITcpFrame BuildResponse( BrowserForm browserForm )
		{
			XmlPageRequestBody xmlPageRequestBody;
			XmlPageRequestFrame xmlPageRequestFrame;

			xmlPageRequestFrame = XmlPageRequestFrame;
			xmlPageRequestBody = xmlPageRequestFrame.XmlPageRequestBody;

			RequestResponseFrame responseFrame;

			NavigateAndSync( browserForm, xmlPageRequestBody );
			//string search = xmlPageRequestBody.ReceiveSearch;

			//	Wait until all navigation results are completed.
			BrowserDocument browserDocument = browserForm.BrowserDocument;
			browserDocument.NavigateManualResetEvent.WaitOne();

			//	Get the results of navigation
			string domText = browserDocument.DomText;
			string xmlText = browserDocument.XmlText;

			List<string> textList = new List<string>
			{
				domText,
				xmlText,
				browserDocument.DomContentOnly(),
				browserDocument.XmlContentOnly()
			};

			//	Frame the results.
			XmlPageReplyFrame xmlPageReplyFrame = new XmlPageReplyFrame( textList );
			responseFrame = new RequestResponseFrame( xmlPageReplyFrame.ToByteArray() );

			//	Navigate to a blank page so the pool of browsers remains quiet.
			NavigateToBlank( browserForm );

			return ( responseFrame );
		}
	}
}

