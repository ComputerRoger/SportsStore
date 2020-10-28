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

namespace BrowserFormServer
{
	public class MainForm : Form
	{
		private readonly System.ComponentModel.IContainer components = null;
		private Label label1;
		private Label label2;
		private readonly IAppDocument m_AppDocument;

		public MainForm( IAppDocument appDocument )
		{
			m_AppDocument = appDocument;

			InitializeComponent();
			HookupEvents();
		}

		#region Propterties.

		protected IAppDocument AppDocument => ( m_AppDocument );
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
			this.Load += new System.EventHandler(this.MainForm_Load_1);
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

		public BrowserForm StartBrowserForm()
		{
			BrowserForm browserForm;

			//	Create a modeless window.
			browserForm = new BrowserForm( AppDocument )
			{
				Owner = this
			};

			browserForm.Show();
			return ( browserForm );
		}
		#endregion

		private void MainForm_Load_1( object sender, EventArgs e )
		{

		}
	}
}

