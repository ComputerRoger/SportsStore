using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Threading;
using GeneralClassLibrary;
using AsyncSockets;

namespace BrowserFormServer
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			TrialAndError.TestTrialAndError();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );

			//	Create the application document to maintain state.
			AppDocument appDocument;
			MainForm mainForm;

			appDocument = new AppDocument();
			mainForm = appDocument.MainForm;

			//	Provide a container for the TCP/IP server.
			int serverPort = BrowserFormServer.Properties.Settings.Default.BrowserServerPort;
			int sizeThreadPool = BrowserFormServer.Properties.Settings.Default.SizeThreadPool;
			IThreadDispatcher threadDispatcher = new PoolThreadDispatcher( sizeThreadPool );
			IProtocolFactory protocolFactory = new BrowserServerProtocolFactory();
			ILogger logger = new ConsoleLogger();
			TcpServer tcpServer = new TcpServer( serverPort, threadDispatcher, protocolFactory, logger, appDocument );

			//	Serve TCP/IP on a separate thread.
			System.Threading.Thread serverThread;
			ThreadStart threadStart = new ThreadStart( tcpServer.StartMethod );
			serverThread = new Thread( threadStart );
			serverThread.Start();

			//	The MainForm, as the root control container, provides user interface capability.
			Application.Run( mainForm );
		}
	}
}

