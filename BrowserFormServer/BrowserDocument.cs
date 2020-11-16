using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace BrowserFormServer
{
	public interface IBrowserDocument
	{
		String DomText { get; set; }
		String XmlText { get; set; }
		string DomNoScript();
		string XmlNoScript();
		Uri Uri { get; set; }
		ManualResetEvent NavigateManualResetEvent { get; set; }
	}
	public class BrowserDocument : IBrowserDocument
	{
		Uri m_Uri;
		string m_DomText;
		string m_XmlText;
		public ManualResetEvent NavigateManualResetEvent { get; set; }

		public BrowserDocument()
		{
			NavigateManualResetEvent = new ManualResetEvent( false );
		}

		public String DomText
		{
			get
			{
				return ( m_DomText );
			}
			set
			{
				m_DomText = value;
			}
		}
		public String XmlText
		{
			get
			{
				return ( m_XmlText );
			}
			set
			{
				m_XmlText = value;
			}
		}

		public String DomNoScript() => RemoveScriptTags( DomText );
		public String DomContentOnly() => RemoveNonContent( DomText );
		public String XmlNoScript() => RemoveScriptTags( XmlText );
		public String XmlContentOnly() => RemoveNonContent( XmlText );

		public Uri Uri
		{
			get
			{
				return ( m_Uri );
			}
			set
			{
				m_Uri = value;
			}
		}

		//	RegexOptions.Singleline
		///	Use single-line mode, where the period(.) matches every character including \n. ( instead of every character except \n). 

		public static string RemoveTags( string rawText, string tagText )
		{
			string pattern;
			string text;

			pattern = @"<" + tagText + @"((?!<" + tagText + @").)*?/" + tagText + @">";
			text = Regex.Replace( rawText, pattern, @"", RegexOptions.Singleline | RegexOptions.IgnoreCase );
			return ( text );
		}

		public static string RemoveStartThruEnd( string rawText, string startText, string endText )
		{
			string pattern;
			string text;

			pattern = startText + @"((?!<" + startText + @").)*?/" + endText;
			text = Regex.Replace( rawText, pattern, @"", RegexOptions.Singleline | RegexOptions.IgnoreCase );
			return ( text );
		}

		public static string RemoveNoscriptTags( string rawText )
		{
			string text;

			text = RemoveTags( rawText, "Noscript" );
			return ( text );
		}
		public static string RemoveStyleTags( string rawText )
		{
			string text;

			text = RemoveTags( rawText, "Style" );
			return ( text );
		}

		public static string RemoveScriptTags( string rawText )
		{
			string text;

			text = RemoveTags( rawText, "Script" );
			return ( text );
		}

		public static string RemoveScript1Tags( string rawText )
		{
			string text;

			text = rawText;
			text = Regex.Replace( text, @"<script((?!<script).)*?/script>", @"", RegexOptions.Singleline | RegexOptions.IgnoreCase );
			return ( text );
		}


		public static string RemoveMetaTags( string rawText )
		{
			string text;

			text = rawText;
			//text = RemoveStartThruEnd( text, @"<Meta", @">" );

			text = Regex.Replace( text, "<meta((?!<).)*?>", @"", RegexOptions.Singleline | RegexOptions.IgnoreCase );
			return ( text );
		}

		//	Remove comments <!-- thru -->
		public static string RemoveComments( string rawText )
		{
			string text;

			text = rawText;
			text = Regex.Replace( text, @"<!--[\s\S]*?-->", "", RegexOptions.Singleline | RegexOptions.IgnoreCase );
			return ( text );
		}

		//	Remove accumulated spaces.
		public static string RemoveWhiteSpace( string rawText )
		{
			string text;

			text = rawText;
			
			//	Remove leading white space from each line.
			text = Regex.Replace( text, @"^(\s)+", "", RegexOptions.Multiline );

			//	Remove trailing white space from each line.
			text = Regex.Replace( text, @"(\s)+$", "", RegexOptions.Multiline );

			//	Remove all newlines because the XML viewer does its own formatting.
			text = Regex.Replace( text, @"[\n]*", "", RegexOptions.Singleline );

			return ( text );            
		}

		public static string RemoveNonContent( string rawText )
		{
			string text;

			text = RemoveScript1Tags( rawText );

			text = RemoveScriptTags( text );

			text = RemoveMetaTags( text );

			text = RemoveNoscriptTags( text );

			text = RemoveStyleTags( text );

			text = RemoveComments( text );

			text = RemoveWhiteSpace( text );
			return ( text );
		}
	}
}
