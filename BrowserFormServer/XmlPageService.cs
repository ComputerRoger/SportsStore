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
			ITcpFrame responseFrame = BuildResponse( browserForm, appDocument );

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
		public ITcpFrame BuildResponse( BrowserForm browserForm, AppDocument appDocument )
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

			List<string> textList = new List<string>();

			textList.Add( domText );
			textList.Add( xmlText );
			textList.Add( browserDocument.DomContentOnly() );
			textList.Add( browserDocument.XmlContentOnly() );

			//	Frame the results.
			XmlPageReplyFrame xmlPageReplyFrame = new XmlPageReplyFrame( textList );
			responseFrame = new RequestResponseFrame( xmlPageReplyFrame.ToByteArray() );

			//	Navigate to a blank page so the pool of browsers remains quiet.
			NavigateToBlank( browserForm );

			return ( responseFrame );
		}
	}
}



//	HtmlDecode works.	Must be lowercase.
//string sampleHtml = "&lt;strong&gt;bold &LT/strong&GT;test";
//var resultHtml = System.Web.HttpUtility.HtmlDecode( sampleHtml );
// result = "<strong>bold </strong>test"


//	UrlDecode works.
//string sampleUrl = "test %3cstrong%3ebold %3c/strong%3etest";
//string sampleUrl = "test %3cstrong%3ebold %3C/strong%3Etest";
//var resultUrl = System.Web.HttpUtility.UrlDecode( sampleUrl );
// result = "<strong>bold </strong>test"


//string sampleUrl = "test %3cstrong%3ebold %3c/strong%3etest";
//string sampleUrl = "https://images-na.ssl-images-amazon.com/images/I/41icwgAxVqL._RC|71RKo6SPKEL.css,21qFIynv1ZL.css,31FX6DlOvlL.css,21lRUdwotiL.css,41oKRlyPnmL.css,11G4HxMtMSL.css,31OvHRW XiL.css,01XHMOHpK1L.css_.css?AUIClients/NavDesktopUberAsset&amp;yKDHSa d#desktop.309131-T1";
//var resultUrl = System.Web.HttpUtility.UrlDecode( sampleUrl );
