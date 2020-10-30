using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AsyncSockets;

namespace BrowserFormServer
{
	public class AppDocument : IAppDocument
	{
		public AppDocument()
		{
			MainForm = new MainForm( this );
			ActiveBrowserForms = new List<BrowserForm>();
			PoolBrowserForms = new Stack<BrowserForm>();
		}


		public MainForm MainForm { get; protected set; }

		public List<BrowserForm> ActiveBrowserForms { get; protected set; }
		public Stack<BrowserForm> PoolBrowserForms { get; protected set; }
	}
}
