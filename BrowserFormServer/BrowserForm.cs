using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MtrDev.WebView2.Winforms;
using MtrDev.WebView2.Wrapper;
using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.IO;

namespace BrowserFormServer
{
	public partial class BrowserForm : Form
	{
		public BrowserForm( AppDocument appDocument )
		{
			AppDocument = appDocument;
			BrowserDocument = new BrowserDocument();

			InitializeComponent();
			SecureBrowserControl();
			HookupEvents();
		}

		#region Propterties.

		public AppDocument AppDocument { get; protected set; }
		public BrowserDocument BrowserDocument { get; protected set; }
		public string NavigationResults { get; protected set; }
		#endregion

		#region Methods.

		protected void SecureBrowserControl()
		{
			bool isWebMessageEnabled = BrowserControl.IsWebMessageEnabled;
			bool isScriptEnabled = BrowserControl.IsScriptEnabled;
			bool areDefaultScriptDialogsEnabled = BrowserControl.AreDefaultScriptDialogsEnabled;
		}

		public delegate string BrowserWorkDelegate( string url );

		//	The delegate should match this method.
		public string BrowserWork( string url )
		{
			string resultText;

			//	Prepare to wait for results.

			BrowserDocument.NavigateManualResetEvent = new ManualResetEvent( false );
			BrowserDocument.NavigateManualResetEvent.Reset();
			resultText = "";
			NavigationResults = "";

			//	Start the navigation process.
			Navigate( url );

			//	The event handlers will save the results.
			//	After saving the results, the event handlers will set the ManualResetEvent.

			return ( resultText );
		}

		public void Navigate( string url )
		{
			BrowserControl.Navigate( url );
		}

		public string HtmlToXml( string htmlText )
		{
			HtmlAgilityPack.HtmlDocument htmlDocument;
			string xmlText;

			//	Create an HTML document.
			htmlDocument = new HtmlAgilityPack.HtmlDocument();
			htmlDocument.LoadHtml( htmlText );

			//	Use HTML Agility Pack to transform the HTML to XML.
			htmlDocument.OptionOutputAsXml = true;
			System.IO.TextWriter stringWriter;
			stringWriter = new System.IO.StringWriter();
			htmlDocument.Save( stringWriter );
			xmlText = stringWriter.ToString();

			return xmlText;
		}

		//	This callback method is executed after ExecuteScript is complete.
		public void GetDomText( ExecuteScriptCompletedEventArgs executeScriptCompletedEventArgs )
		{
			IBrowserDocument browserDocument;
			string jsonText;

			browserDocument = this.BrowserDocument;

			jsonText = executeScriptCompletedEventArgs.ResultAsJson;

			//	Replace the escaped text with normal characters.
			string unescapedText = System.Text.RegularExpressions.Regex.Unescape( jsonText );

			//	Remove the surrounding quotes.
			string htmlText = unescapedText.Substring( 1, unescapedText.Length - 2 );

			//	Remove browser encoding.
			htmlText = System.Net.WebUtility.HtmlDecode( htmlText );
			htmlText = System.Net.WebUtility.UrlDecode( htmlText );

			browserDocument.DomText = htmlText;

			string xmlText;
			xmlText = HtmlToXml( htmlText );
			browserDocument.XmlText = xmlText;

			//	Signal that navigation has completed and results have been parsed.
			NavigationResults = browserDocument.DomText;
			BrowserDocument.NavigateManualResetEvent.Set();
		}

		protected void NavigationCompleted( object sender, MtrDev.WebView2.Wrapper.NavigationCompletedEventArgs e )
		{
			Action<ExecuteScriptCompletedEventArgs> scriptAction;
			string javascriptText;

			scriptAction = new Action<ExecuteScriptCompletedEventArgs>( GetDomText );
			javascriptText = "document.documentElement.innerHTML";
			this.BrowserControl.ExecuteScript( javascriptText, scriptAction );
		}
		#endregion

		#region Events.

		protected void HookupEvents()
		{
			BrowserControl.NavigationCompleted += BrowserControl_NavigationCompleted;
			BrowserControl.ScriptDialogOpening += BrowserControl_ScriptDialogOpening;
			BrowserControl.WebMessageRecieved += BrowserControl_WebMessageRecieved;
			BrowserControl.ProcessFailed += BrowserControl_ProcessFailed;
			BrowserControl.PermissionRequested += BrowserControl_PermissionRequested;
			BrowserControl.ContentLoading += BrowserControl_ContentLoading;
		}

		private void BrowserControl_ContentLoading( object sender, ContentLoadingEventArgs e )
		{
			//	1.	Navigation Starting.
			//	2.	Source Changed.
			//	3.	ContentLoading.
			//	4.	HistoryChanged.
			//	5.	NavigationCompleted.
		}

		private void BrowserControl_PermissionRequested( object sender, PermissionRequestedEventArgs e )
		{
			//	Log this.
		}

		private void BrowserControl_ProcessFailed( object sender, ProcessFailedEventArgs e )
		{
			//	Log this.
		}

		private void BrowserControl_WebMessageRecieved( object sender, WebMessageReceivedEventArgs e )
		{
			//	Perhaps look for this event.
		}

		private void BrowserControl_ScriptDialogOpening( object sender, ScriptDialogOpeningEventArgs e )
		{
			//	Capture Captcha here.
		}

		private void BrowserForm_Load( object sender, EventArgs e )
		{
		}

		private void BrowserControl_NavigationCompleted( object sender, MtrDev.WebView2.Wrapper.NavigationCompletedEventArgs e )
		{
			NavigationCompleted( sender, e );
		}
		#endregion
	}
}

//BrowserControl.Size = new Size( 0,0 );
//Navigate( @"https://this-page-intentionally-left-blank.org/" );
//Navigate( @"https://www.amazon.com" );
