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
		//	This class contains the forms as a single object to be locked.
		protected class FormsCollection
		{
			public FormsCollection()
			{
				ActiveBrowserForms = new List<BrowserForm>();
				PoolBrowserForms = new Stack<BrowserForm>();
			}
			public List<BrowserForm> ActiveBrowserForms { get; set; }
			public Stack<BrowserForm> PoolBrowserForms { get; set; }
		}

		public AppDocument()
		{
			MainForm = new MainForm( this );
			BrowserFormCollection = new FormsCollection();
		}

		public MainForm MainForm { get; protected set; }
		protected FormsCollection BrowserFormCollection { get; set; }

		public BrowserForm StartBrowserForm( MainForm mainForm )
		{
			BrowserForm browserForm;

			lock( BrowserFormCollection )
			{
				if( BrowserFormCollection.PoolBrowserForms.Count > 0 )
				{
					browserForm = BrowserFormCollection.PoolBrowserForms.Pop();
				}
				else
				{
					//	Create a modeless window.
					browserForm = new BrowserForm( this )
					{
						Owner = mainForm
					};
				}
				browserForm.Show();

				BrowserFormCollection.ActiveBrowserForms.Add( browserForm );
			}
			return ( browserForm );
		}
		public bool StopBrowserForm( BrowserForm browserForm )
		{
			bool isRemove;

			lock( BrowserFormCollection )
			{
				isRemove = BrowserFormCollection.ActiveBrowserForms.Remove( browserForm );
				browserForm.Hide();

				if( BrowserFormCollection.PoolBrowserForms.Count > Properties.Settings.Default.SizeBrowserPool )
				{
					//	When a form is closed, all resources created within the object are closed and the form is disposed.
					browserForm.Close();
				}
				else
				{
					BrowserFormCollection.PoolBrowserForms.Push( browserForm );
				}
			}
			return ( isRemove );
		}
	}
}
