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
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics;

namespace BrowserFormServer
{
	public class MainForm : Form
	{
		private readonly System.ComponentModel.IContainer components = null;
		private Label label1;
		private Label label2;

		public MainForm( AppDocument appDocument )
		{
			AppDocument = appDocument;

			InitializeComponent();
			HookupEvents();
		}

		#region Propterties.

		public AppDocument AppDocument { get; protected set; }
		#endregion

		#region Methods.

		private void InitializeComponent()
		{
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.ForestGreen;
			this.label1.Location = new System.Drawing.Point(100, 85);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(196, 24);
			this.label1.TabIndex = 0;
			this.label1.Text = "This is the Main Form.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.ForeColor = System.Drawing.Color.OrangeRed;
			this.label2.Location = new System.Drawing.Point(103, 127);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(210, 24);
			this.label2.TabIndex = 1;
			this.label2.Text = "Do NOT close this form.";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.ClientSize = new System.Drawing.Size(408, 254);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		protected override void Dispose( bool disposing )
		{
			if( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}
		#endregion

		#region Event Handlers.

		protected void HookupEvents()
		{
			this.Load += MainForm_Load;
		}

		private void MainForm_Load( object sender, EventArgs e )
		{
			//this.ClientSize = new Size( 300, 0 );
		}
		#endregion

		#region Methods.
		public delegate BrowserForm StartBrowserFormDelegate();
		public delegate bool StopBrowserFormDelegate( BrowserForm browserForm );

		public BrowserForm StartBrowserForm()
		{
			BrowserForm browserForm;

			if( AppDocument.PoolBrowserForms.Count > 0 )
			{
				browserForm = AppDocument.PoolBrowserForms.Pop();
			}
			else
			{
				//	Create a modeless window.
				browserForm = new BrowserForm( this.AppDocument )
				{
					Owner = this
				};
			}
			browserForm.Show();

			AppDocument.ActiveBrowserForms.Add( browserForm );

			return ( browserForm );
		}
		public bool StopBrowserForm( BrowserForm browserForm )
		{
			bool isRemove;

			isRemove = AppDocument.ActiveBrowserForms.Remove( browserForm );
			Debug.Assert( isRemove, "Could not remove a browser form from the active list." );
			browserForm.Hide();

			if( AppDocument.PoolBrowserForms.Count > Properties.Settings.Default.SizeBrowserPool )
			{
				//	When a form is closed, all resources created within the object are closed and the form is disposed.
				browserForm.Close();
			}
			else
			{
				AppDocument.PoolBrowserForms.Push( browserForm );
			}

			return ( isRemove );
		}
		#endregion
	}
}

