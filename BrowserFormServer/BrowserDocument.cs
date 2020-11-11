﻿using System;
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

		//public static void test()
		//{
		//	Regex regex;
		//	regex = new Regex( @"<script((?!<script).)*?/script>", RegexOptions.Singleline | RegexOptions.IgnoreCase |RegexOptions.IgnorePatternWhitespace );
		//	string text;
		//	string text1;
		//	string text2;

		//	text = "<script   FirstText   /script> This is stuff between\n scripts. <script   Some  text \n   /script>";
		//	string matchValue = regex.Match( text ).Value;
		//	text1 = regex.Replace( text, "" );
		//	text2 = Regex.Replace( text, @"<script((?!<script).)*?/script>", @"", RegexOptions.Singleline | RegexOptions.IgnoreCase );
		//}

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

		//	Remove accumulated spaces.
		public static string RemoveWhiteSpace( string rawText )
		{
			string text;

			text = rawText;
			text = Regex.Replace( text, @"^(?:[\t ]*(?:\r ?\n|\r))+", @"\n", RegexOptions.Multiline | RegexOptions.IgnoreCase );
			return ( text );            
		}

		//	Remote lines that are just newlines.
		public static string RemoveNewLines( string rawText )
		{
			string text;

			text = rawText;
			text = Regex.Replace( text, "^(?:[\t ]*(?:\r?\n|\r))+", "", RegexOptions.Multiline | RegexOptions.IgnoreCase );
			return ( text );
		}

		//	Remove comments <!-- thru -->
		public static string RemoveComments( string rawText )
		{
			string text;

			text = rawText;
			text = Regex.Replace( text, @"<!--[\s\S]*?-->", "", RegexOptions.Singleline | RegexOptions.IgnoreCase );
			//xmlFileContent.replaceAll( "<!--[\s\S]*?-->", "" );
			return ( text );
		}

		public static string RemoveNonContent( string rawText )
		{
			string text;

			text = rawText;

			//test();

			text = RemoveScript1Tags( rawText );

			text = RemoveScriptTags( text );

			text = RemoveMetaTags( text );

			text = RemoveNoscriptTags( text );

			text = RemoveStyleTags( text );

			text = RemoveWhiteSpace( text );

			text = RemoveNewLines( text );

			text = RemoveComments( text );
			return ( text );
		}
		/**
//<![CDATA[
@-webkit-keyframes gb__a{0%{opacity:0}50%{opacity:1}}@keyframes gb__a{0%{opacity:0}50%{opacity:1}}.gb_nd{display:inline-block;padding:0 0 0 15px;vertical-align:middle}.gb_nd:first-child,#gbsfw:first-child+.gb_nd{padding-left:0}.gb_0f{position:relative}.gb_D{display:inline-block;outline:none;vertical-align:middle;-webkit-border-radius:2px;border-radius:2px;-webkit-box-sizing:border-box;box-sizing:border-box;height:30px;width:30px;color:#000;cursor:pointer;text-decoration:none}#gb#gb a.gb_D{color:#000;cursor:pointer;text-decoration:none}.gb_7a{border-color:transparent;border-bottom-color:#fff;border-style:dashed dashed solid;border-width:0 8.5px 8.5px;display:none;position:absolute;left:6.5px;top:37px;z-index:1;height:0;width:0;-webkit-animation:gb__a .2s;animation:gb__a .2s}.gb_8a{border-color:transparent;border-style:dashed dashed solid;border-width:0 8.5px 8.5px;display:none;position:absolute;left:6.5px;z-index:1;height:0;width:0;-webkit-animation:gb__a .2s;animation:gb__a .2s;border-bottom-color:#ccc;border-bottom-color:rgba(0,0,0,.2);top:36px}x:-o-prefocus,div.gb_8a{border-bottom-color:#ccc}.gb_F{background:#fff;border:1px solid #ccc;border-color:rgba(0,0,0,.2);color:#000;-webkit-box-shadow:0 2px 10px rgba(0,0,0,.2);box-shadow:0 2px 10px rgba(0,0,0,.2);display:none;outline:none;overflow:hidden;position:absolute;right:0;top:44px;-webkit-animation:gb__a .2s;animation:gb__a .2s;-webkit-border-radius:2px;border-radius:2px;-webkit-user-select:text}.gb_nd.gb_oa .gb_7a,.gb_nd.gb_oa .gb_8a,.gb_nd.gb_oa .gb_F,.gb_oa.gb_F{display:block}.gb_nd.gb_oa.gb_1f .gb_7a,.gb_nd.gb_oa.gb_1f .gb_8a{display:none}.gb_2f{position:absolute;right:0;top:44px;z-index:-1}.gb_Ra .gb_7a,.gb_Ra .gb_8a,.gb_Ra .gb_F{margin-top:-10px}.gb_C .gb_D{background-position:-132px -38px;opacity:.55}.gb_E .gb_C .gb_D{background-position:-132px -38px}.gb_j .gb_C .gb_D{background-position:-463px -35px;opacity:1}.gb_F.gb_H{min-height:196px;overflow-y:auto;width:320px}.gb_F.gb_H.gb_l{-webkit-border-radius:8px;border-radius:8px;-webkit-box-shadow:0 1px 2px 0 rgba(60,64,67,.30),0 2px 6px 2px rgba(60,64,67,.15);box-shadow:0 1px 2px 0 rgba(60,64,67,.30),0 2px 6px 2px rgba(60,64,67,.15);width:328px}.gb_I{-webkit-transition:height .2s ease-in-out;transition:height .2s ease-in-out}.gb_I.gb_H.gb_l{-webkit-transition:height 1s ease-in-out;transition:height 1s ease-in-out}.gb_J{background:#fff;margin:0;padding:28px;padding-right:27px;text-align:left;white-space:normal;width:265px}.gb_J:not(.gb_l){min-height:100px}.gb_H.gb_l&gt;.gb_J{padding:12px 14px 20px 14px;width:300px}.gb_K{background:#f5f5f5;cursor:pointer;height:40px;overflow:hidden}.gb_L{position:relative}.gb_K{display:block;line-height:40px;text-align:center;width:320px}.gb_L{display:block;line-height:40px;text-align:center}.gb_L.gb_M{line-height:0}.gb_K,.gb_K:visited,.gb_K:active,.gb_L,.gb_L:visited{color:rgba(0,0,0,0.87);text-decoration:none}.gb_L:active{color:rgba(0,0,0,0.87)}#gb a.gb_K,#gb a.gb_K:visited,#gb a.gb_K:active,#gb a.gb_L,#gb a.gb_L:visited{color:rgba(0,0,0,0.87);text-decoration:none}#gb a.gb_L:active{color:rgba(0,0,0,0.87)}.gb_L:not(.gb_l),.gb_J:not(.gb_l){display:none}.gb_A,.gb_J.gb_A:not(.gb_l),.gb_A+.gb_L,.gb_N .gb_L,.gb_N .gb_J{display:block}.gb_l .gb_O,.gb_l .gb_P,.gb_N .gb_O,.gb_N .gb_P{display:inline-block}.gb_L:hover,.gb_L:active,#gb a.gb_L:hover,#gb a.gb_L:active{text-decoration:underline}.gb_L{border-bottom:1px solid #ebebeb;left:28px;width:264px}.gb_l .gb_L{border-bottom:1px solid #e8eaed;left:0;width:328px}.gb_Q{text-align:center}a.gb_O,a.gb_P{background-color:#ffffff;border:1px solid #dadce0;-webkit-border-radius:4px;border-radius:4px;-webkit-box-sizing:border-box;box-sizing:border-box;color:#1a73e8;display:inline-block;font:500 14px/16px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;margin:16px 0 18px 0;max-width:264px;outline:none;overflow:hidden;padding:10px 24px;position:static;text-align:center;text-decoration:none;text-overflow:ellipsis;white-space:nowrap}a.gb_O:visited,a.gb_P:visited{color:#1a73e8}.gb_P:hover,.gb_O:hover{background-color:#f8fbff;border-color:#cce0fc;text-decoration:none}.gb_P:focus,.gb_P:hover:focus,.gb_O:focus,.gb_O:hover:focus{background-color:#f4f8ff;border-color:#c9ddfc}.gb_P:active,.gb_P:active:focus,.gb_O:active,.gb_O:active:focus{background-color:#ecf3fe;border-color:transparent;-webkit-box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15);box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15);text-decoration:none}.gb_N .gb_K{display:none}.gb_L:last-child{border-bottom-width:0}.gb_o .gb_f{display:initial}.gb_o.gb_R{height:100px;text-align:center}.gb_o.gb_R img{padding:34px 0;height:32px;width:32px}.gb_o .gb_r+img{border:0;margin:8px;height:48px;width:48px}.gb_o div.gb_S{background:#ffa;-webkit-border-radius:5px;border-radius:5px;padding:5px;text-align:center}.gb_l.gb_o.gb_T,.gb_l.gb_o.gb_p,.gb_o.gb_T,.gb_o.gb_p{padding-bottom:0}.gb_l.gb_o.gb_q,.gb_l.gb_o.gb_p,.gb_o.gb_q,.gb_o.gb_p{padding-top:0}.gb_o.gb_p a,.gb_o.gb_q a{top:0}.gb_U .gb_K{margin-top:0;position:static}.gb_V{display:inline-block}.gb_W:hover,#gb#gb .gb_W:hover{text-decoration:underline}.gb_X .gb_J{position:relative}.gb_X .gb_l&gt;.gb_f{top:20px;left:20px}.gb_X .gb_f{position:absolute;top:28px;left:28px}.gb_K.gb_Z{display:none;height:0}.gb_a-a{width:100%;height:100%;border:0;overflow:hidden}.gb_a.gb_b-b-c{position:absolute;top:0;left:0;background-color:#fff}.gb_a.gb_b-b{position:absolute;top:0;left:0;background-color:#fff;border:1px solid #acacac;width:auto;padding:0;z-index:1001;overflow:auto;-webkit-box-shadow:rgba(0,0,0,.2) 0 4px 16px;-webkit-box-shadow:rgba(0,0,0,.2) 0 4px 16px;box-shadow:rgba(0,0,0,.2) 0 4px 16px;-webkit-transition:top .5s ease-in-out;-webkit-transition:top .5s ease-in-out;transition:top .5s ease-in-out}.gb_a-d{position:absolute;z-index:1002}.gb_a.gb_b-b-e{font-size:0;padding:0}.gb_a.gb_b-b-f{height:0;margin:0}.gb_a.gb_b-b-f-g,.gb_a.gb_b-b-h{display:none}.gb_La{-webkit-background-size:32px 32px;background-size:32px 32px;border:0;-webkit-border-radius:50%;border-radius:50%;display:block;margin:-1px;position:relative;height:32px;width:32px;z-index:0}.gb_Ma{background-color:#e8f0fe;border:1px solid rgba(32,33,36,.08);position:relative}.gb_Ma.gb_La{height:30px;width:30px}.gb_Ma.gb_La:hover,.gb_Ma.gb_La:active{-webkit-box-shadow:none;box-shadow:none}.gb_Na{background:#fff;border:none;-webkit-border-radius:50%;border-radius:50%;bottom:2px;-webkit-box-shadow:0 1px 2px 0 rgba(60,64,67,.30),0 1px 3px 1px rgba(60,64,67,.15);box-shadow:0 1px 2px 0 rgba(60,64,67,.30),0 1px 3px 1px rgba(60,64,67,.15);height:14px;margin:2px;position:absolute;right:0;width:14px}.gb_Oa{color:#1f71e7;font:400 22px/32px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;text-align:center;text-transform:uppercase}@media (min-resolution:1.25dppx),(-o-min-device-pixel-ratio:5/4),(-webkit-min-device-pixel-ratio:1.25),(min-device-pixel-ratio:1.25){.gb_La::before{display:inline-block;-webkit-transform:scale(.5);transform:scale(.5);-webkit-transform-origin:left 0;transform-origin:left 0}.gb_Pa::before{display:inline-block;-webkit-transform:scale(.5);transform:scale(.5);-webkit-transform-origin:left 0;transform-origin:left 0}.gb_l .gb_Pa::before{-webkit-transform:scale(0.416666667);transform:scale(0.416666667)}}.gb_La:hover,.gb_La:focus{-webkit-box-shadow:0 1px 0 rgba(0,0,0,.15);box-shadow:0 1px 0 rgba(0,0,0,.15)}.gb_La:active{-webkit-box-shadow:inset 0 2px 0 rgba(0,0,0,.15);box-shadow:inset 0 2px 0 rgba(0,0,0,.15)}.gb_La:active::after{background:rgba(0,0,0,.1);-webkit-border-radius:50%;border-radius:50%;content:'';display:block;height:100%}.gb_Qa{cursor:pointer;line-height:30px;min-width:30px;opacity:.75;overflow:hidden;vertical-align:middle;text-overflow:ellipsis}.gb_D.gb_Qa{width:auto}.gb_Qa:hover,.gb_Qa:focus{opacity:.85}.gb_Ra .gb_Qa,.gb_Ra .gb_Sa{line-height:26px}#gb#gb.gb_Ra a.gb_Qa,.gb_Ra .gb_Sa{font-size:11px;height:auto}.gb_Ta{border-top:4px solid #000;border-left:4px dashed transparent;border-right:4px dashed transparent;display:inline-block;margin-left:6px;opacity:.75;vertical-align:middle}.gb_Ua:hover .gb_Ta{opacity:.85}.gb_ja&gt;.gb_Va{padding:3px 3px 3px 4px}.gb_Wa.gb_Ka{color:#fff}.gb_j .gb_Qa,.gb_j .gb_Ta{opacity:1}#gb#gb.gb_j.gb_j a.gb_Qa,#gb#gb .gb_j.gb_j a.gb_Qa{color:#fff}.gb_j.gb_j .gb_Ta{border-top-color:#fff;opacity:1}.gb_E .gb_La:hover,.gb_j .gb_La:hover,.gb_E .gb_La:focus,.gb_j .gb_La:focus{-webkit-box-shadow: 0 1px 0 rgba(0,0,0,.15) , 0 1px 2px rgba(0,0,0,.2) ;box-shadow: 0 1px 0 rgba(0,0,0,.15) , 0 1px 2px rgba(0,0,0,.2) }.gb_Xa .gb_Va,.gb_Za .gb_Va{position:absolute;right:1px}.gb_Va.gb_i,.gb_0a.gb_i,.gb_Ua.gb_i{-webkit-flex:0 1 auto;flex:0 1 auto;-webkit-flex:0 1 main-size;flex:0 1 main-size}.gb_1a.gb_2a .gb_Qa{width:30px!important}.gb_3a.gb_Ka{display:none}.gb_4a{height:40px;position:absolute;right:-5px;top:-5px;width:40px}.gb_5a .gb_4a,.gb_6a .gb_4a{right:0;top:0}.gb_Qa~.gb_7a,.gb_Qa~.gb_8a{left:auto;right:6.5px}.gb_9a{outline:none;-webkit-transform:translateZ(0);transform:translateZ(0)}.gb_l.gb_9a{-webkit-border-radius:8px;border-radius:8px;margin-left:12px}@media screen and (min-width:361px){.gb_l.gb_9a{width:354px}}@media screen and (max-width:361px){.gb_l.gb_9a{width:calc(100vw -  12px *2)}}.gb_l.gb_9a.gb_ab{max-height:-webkit-calc(100vh - 44px - 100px);max-height:calc(100vh - 44px - 100px)}.gb_l.gb_9a.gb_bb{max-height:-webkit-calc(100vh - 44px - 15px - 100px);max-height:calc(100vh - 44px - 15px - 100px)}.gb_l.gb_9a.gb_cb{background-color:#2d2e30}.gb_db.gb_eb{color:#5f6368;font:400  12px / 16px  Roboto,RobotoDraft,Helvetica,Arial,sans-serif}.gb_fb.gb_db.gb_eb{background-color:rgba(138,180,248,0.24);color:#e8eaed}.gb_gb,#gb a.gb_gb.gb_gb,.gb_hb a,#gb .gb_hb.gb_hb a{color:#36c;text-decoration:none}.gb_db&gt;.gb_gb,#gb .gb_db&gt;a.gb_gb.gb_gb{color:#0070ff;font:inherit;font-weight:500;outline:0}.gb_fb.gb_db&gt;.gb_gb,#gb .gb_fb.gb_db&gt;a.gb_gb.gb_gb{color:#8ab4f8}.gb_gb:active,#gb a.gb_gb.gb_gb:active,.gb_gb:hover,#gb a.gb_gb.gb_gb:hover,.gb_hb a:active,#gb .gb_hb a:active,.gb_hb a:hover,#gb .gb_hb a:hover,#gb .gb_db&gt;a.gb_gb.gb_gb:focus{text-decoration:underline}.gb_ib{margin:20px;white-space:nowrap}.gb_l&gt;.gb_ib{margin:20px 33px}.gb_jb,.gb_kb{display:inline-block;vertical-align:top}.gb_jb.gb_lb,.gb_kb.gb_mb{vertical-align:middle}.gb_l .gb_jb,.gb_l .gb_kb{display:block;vertical-align:top;text-align:center}.gb_lb{cursor:default}.gb_l .gb_jb{margin-bottom:10px;position:relative;height:86px;width:86px}.gb_nb{-webkit-border-radius:50%;border-radius:50%;overflow:hidden;-webkit-transform:translateZ(0)}.gb_Pa{border:none;margin-right:6px;vertical-align:top;height:80px;width:80px}.gb_ob{margin-bottom:11px;margin-top:4px}@media screen and (min-width:361px){.gb_l .gb_jb,.gb_ob{margin-left:101px}}@media screen and (max-width:361px){.gb_l .gb_jb,.gb_ob{margin-left:calc(( calc(100vw -  12px *2)  -  33px *2 -  86px )/2)}}.gb_pb.gb_pb{fill:#1a73e8}.gb_cb .gb_pb{fill:#8ab4f8}.gb_l .gb_qb.gb_Pa{position:relative;left:2px;margin-right:10px;top:2px;height:76px;width:76px}.gb_l .gb_rb{background:#fff;bottom:0;position:absolute;right:0;overflow:visible;height:32px;width:32px}.gb_l.gb_cb .gb_rb{background:#2d2e30}.gb_sb{bottom:0;-webkit-box-shadow:0 1px 1px 0 rgba(65,69,73,0.3),0 1px 3px 1px rgba(65,69,73,0.15);box-shadow:0 1px 1px 0 rgba(65,69,73,0.3),0 1px 3px 1px rgba(65,69,73,0.15);margin:0 2.5px 3px;outline:0;position:absolute;right:0;height:26px;width:26px}.gb_sb:hover{background-color:#f8faff}.gb_sb:focus,.gb_sb:hover:focus{background-color:#f4f8ff}.gb_sb:active,.gb_sb:focus:active{background-color:#f4f8ff;-webkit-box-shadow:0 1px 3px 0 rgba(60,64,67,0.3),0 4px 8px 3px rgba(60,64,67,0.15);box-shadow:0 1px 3px 0 rgba(60,64,67,0.3),0 4px 8px 3px rgba(60,64,67,0.15)}.gb_sb:hover&gt;svg.gb_tb,.gb_sb:focus&gt;svg.gb_tb,.gb_sb:active&gt;svg.gb_tb{fill:#1a73e8}.gb_ub{font-weight:bold;margin:-4px 0 1px 0;text-overflow:ellipsis;overflow:hidden}.gb_l .gb_ub{color:#202124;font:500 16px/22px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;letter-spacing:.29px;margin:0;text-align:center;text-overflow:ellipsis;overflow:hidden}.gb_l.gb_cb .gb_ub{color:#e8eaed}.gb_wb{color:#666;text-overflow:ellipsis;overflow:hidden}.gb_l .gb_wb{color:#5f6368;font:400 14px/19px Roboto,RobotoDraft,Helvetica,Arial,sans-serif;letter-spacing:normal;text-align:center;text-overflow:ellipsis;overflow:hidden}.gb_l.gb_cb .gb_wb{color:#e8eaed}.gb_mb&gt;.gb_wb{color:#000;font-weight:bold;margin:-4px 0 1px 0;text-overflow:ellipsis;overflow:hidden}.gb_xb{color:#666;font-style:italic;font-weight:500;margin:4px 0;overflow:hidden}.gb_yb{color:#5f6368;font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:14px;line-height:19px;margin-top:4px;text-align:center}.gb_cb .gb_yb{color:#9aa0a6}.gb_zb{font-weight:500}.gb_Ab.gb_Ab{background-color:#ffffff;border:1px solid #dadce0;-webkit-border-radius:100px;border-radius:100px;color:#3c4043;display:inline-block;font:500 14px/16px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;letter-spacing:.25px;margin:16px 0 0;max-width:254px;outline:0;padding:8px 16px;text-align:center;text-decoration:none;text-overflow:ellipsis;overflow:hidden}.gb_cb .gb_Ab.gb_Ab{background-color:#2d2e30;border:1px solid #5f6368;color:#e8eaed}.gb_Ab:hover{background-color:#f7f8f8}.gb_Ab:focus,.gb_Ab:hover:focus{background-color:#f4f4f4}.gb_Ab:active,.gb_Ab:focus:active{background-color:#e8e8e9;border-color:transparent;-webkit-box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15);box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15)}.gb_Bb{color:#5f6368;margin:14px 33px;text-align:center;white-space:normal}.gb_cb .gb_Bb{color:#e8eaed}.gb_Cb.gb_Cb{-webkit-border-radius:4px;border-radius:4px;color:#5f6368;display:inline-block;font:400  12px / 16px  Roboto,RobotoDraft,Helvetica,Arial,sans-serif;outline:0;padding:4px 8px;text-decoration:none;text-align:center;white-space:normal}.gb_cb .gb_Cb.gb_Cb{border:1px solid transparent;color:#e8eaed}.gb_Cb:hover{background-color:#f7f8f8}.gb_Cb:focus,.gb_Cb:hover:focus{background-color:#f4f4f4}.gb_Cb:active,.gb_Cb:active:focus{background-color:#e8e8e9}.gb_kb .gb_3{background:#4d90fe;border-color:#3079ed;font-weight:bold;margin:10px 0 0 0;color:#fff}#gb .gb_kb a.gb_3.gb_3{color:#fff}.gb_kb .gb_3:hover{background:#357ae8;border-color:#2f5bb7}.gb_Db .gb_7a{border-bottom-color:#fef9db}.gb_eb{background:#fef9db;font-size:11px;padding:10px 20px;white-space:normal}.gb_db.gb_eb{background:#e8f0fe;-webkit-border-radius:4px;border-radius:4px;margin:4px;padding:4px 8px;text-align:center}.gb_db.gb_eb&gt;#gbpbt&gt;span{white-space:nowrap;font-weight:500}.gb_eb b,.gb_gb{white-space:nowrap}.gb_Eb.gb_Eb{background-color:#ffffff;color:#3c4043;display:table;font:500 14px/16px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;letter-spacing:.25px;outline:0;padding:14px 41px;text-align:center;text-decoration:none;width:100%}.gb_cb .gb_Eb.gb_Eb{background-color:#2d2e30;border:1px solid transparent;color:#e8eaed;width:270px}.gb_Eb:hover{background-color:#f7f8f8}.gb_Eb:focus,.gb_Eb:hover:focus{background-color:#f4f4f4}.gb_Eb:active,.gb_Eb:focus:active{background-color:#e8e8e9}.gb_Fb{border:none;display:table-cell;vertical-align:middle;height:20px;width:20px}.gb_sb&gt;svg.gb_tb,.gb_Fb&gt;svg.gb_Hb,.gb_Ib&gt;svg.gb_Jb{color:#5f6368;fill:currentColor}.gb_cb .gb_Ib&gt;svg.gb_Jb{fill:#9aa0a6}.gb_cb .gb_sb{border:1px solid transparent;-webkit-box-shadow:0 1px 3px 0 rgba(0,0,0,0.3),0 4px 8px 3px rgba(0,0,0,0.15);box-shadow:0 1px 3px 0 rgba(0,0,0,0.3),0 4px 8px 3px rgba(0,0,0,0.15)}.gb_cb .gb_sb&gt;svg.gb_tb,.gb_cb .gb_Fb&gt;svg.gb_Hb{color:#e8eaed;fill:currentColor}.gb_cb .gb_sb:hover&gt;svg.gb_tb,.gb_cb .gb_sb:focus&gt;svg.gb_tb,.gb_cb .gb_sb:focus:hover&gt;svg.gb_tb,.gb_cb .gb_sb:active&gt;svg.gb_tb{fill:#8ab4f8}.gb_cb .gb_sb:hover{background-color:#353639;-webkit-box-shadow:0 2px 3px 0 rgba(0,0,0,0.3),0 6px 10px 4px rgba(0,0,0,0.15);box-shadow:0 2px 3px 0 rgba(0,0,0,0.3),0 6px 10px 4px rgba(0,0,0,0.15)}.gb_cb .gb_sb:focus,.gb_cb .gb_sb:focus:hover{background-color:#353639;border:1px solid #5f6368;-webkit-box-shadow:0 2px 3px 0 rgba(0,0,0,0.3),0 6px 10px 4px rgba(0,0,0,0.15);box-shadow:0 2px 3px 0 rgba(0,0,0,0.3),0 6px 10px 4px rgba(0,0,0,0.15)}.gb_cb .gb_sb:active{background-color:rgba(255,255,255,0.12);-webkit-box-shadow:0 4px 4px 0 rgba(0,0,0,0.3),0 8px 12px 6px rgba(0,0,0,0.15);box-shadow:0 4px 4px 0 rgba(0,0,0,0.3),0 8px 12px 6px rgba(0,0,0,0.15)}.gb_Kb{display:table-cell;padding:0 74px 0 16px;text-align:left;vertical-align:middle;white-space:normal}.gb_Lb{border-bottom:1px solid #e8eaed;border-top:1px solid #e8eaed;padding:0 17px;text-align:center}.gb_cb .gb_Lb{border-bottom:1px solid #5f6368;border-top:1px solid #5f6368}.gb_Mb.gb_Mb,.gb_Nb.gb_Nb{background-color:#ffffff;border:1px solid #dadce0;-webkit-border-radius:4px;border-radius:4px;display:inline-block;font:500 14px/16px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;letter-spacing:.15px;margin:16px;outline:0;padding:10px 24px;text-align:center;text-decoration:none;white-space:normal}.gb_Mb.gb_Mb{color:#3c4043}.gb_Nb.gb_Nb{color:#1a73e8}.gb_cb .gb_Nb.gb_Nb,.gb_cb .gb_Mb.gb_Mb{background-color:#2d2e30;border:1px solid #5f6368;color:#e8eaed}.gb_Mb:hover{background-color:#f7f8f8}.gb_Mb:focus,.gb_Mb:hover:focus{background-color:#f4f4f4}.gb_Mb:active,.gb_Mb:active:focus{background-color:#e8e8e9;border-color:transparent;-webkit-box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15);box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15)}.gb_Nb:hover{background-color:#f8fbff;border-color:#cce0fc}.gb_Nb:focus,.gb_Nb:hover:focus{background-color:#f4f8ff;border-color:#c9ddfc}.gb_Nb:active,.gb_Nb:active:focus{background-color:#ecf3fe;border-color:transparent;-webkit-box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15);box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15)}.gb_Rb{border-top:1px solid #e8eaed}.gb_cb .gb_Rb{border-top:1px solid #5f6368}.gb_l.gb_9a{overflow-y:auto;overflow-x:hidden}.gb_Ub{border-top:1px solid #ccc;border-top-color:rgba(0,0,0,.2);display:block;outline-offset:-2px;padding:10px 20px;position:relative;white-space:nowrap}.gb_Vb&gt;.gb_Ub{border:none;cursor:pointer;height:35px;outline:0;padding:12px 33px 13px}.gb_Wb .gb_Ub:focus .gb_Xb{outline:1px dotted #fff}.gb_Ub:hover{background:#eee}.gb_Vb&gt;.gb_Ub:hover{background-color:#f7f8f8}.gb_Vb&gt;.gb_Ub:focus,.gb_Vb&gt;.gb_Ub:hover:focus{background-color:#f4f4f4}.gb_Vb&gt;.gb_Ub:active,.gb_Vb&gt;.gb_Ub:focus:active{background-color:#e8e8e9}.gb_cb .gb_Zb:hover,.gb_cb .gb_Eb:hover,.gb_cb .gb_Cb:hover,.gb_cb .gb_Vb&gt;.gb_Ub:hover{background-color:rgba(255,255,255,0.04);border:1px solid transparent}.gb_cb .gb_Mb:hover,.gb_cb .gb_Ab:hover{background-color:rgba(232,234,237,0.04);border:1px solid #5f6368}.gb_cb .gb_Zb:focus,.gb_cb .gb_Zb:hover:focus,.gb_cb .gb_Ab:focus,.gb_cb .gb_Ab:hover:focus,.gb_cb .gb_Eb:focus,.gb_cb .gb_Eb:hover:focus,.gb_cb .gb_Mb:focus,.gb_cb .gb_Mb:hover:focus,.gb_cb .gb_Vb&gt;.gb_Ub:focus,.gb_cb .gb_Vb&gt;.gb_Ub:hover:focus{background-color:rgba(232,234,237,0.12);border:1px solid #e8eaed}.gb_cb .gb_Cb:focus,.gb_cb .gb_Cb:hover:focus{background-color:rgba(232,234,237,0.12)}.gb_cb .gb_Zb:active,.gb_cb .gb_Zb:focus:active,.gb_cb .gb_Eb:active,.gb_cb .gb_Eb:focus:active,.gb_cb .gb_Cb:active,.gb_cb .gb_Cb:active:focus,.gb_cb .gb_Vb&gt;.gb_Ub:active,.gb_cb .gb_Vb&gt;.gb_Ub:focus:active{background-color:rgba(232,234,237,0.1);border:1px solid transparent}.gb_0b{overflow-x:hidden}.gb_cb .gb_Vb&gt;.gb_Ub{border:1px solid transparent}.gb_cb .gb_Mb:active,.gb_cb .gb_Mb:active:focus,.gb_cb .gb_Ab:active,.gb_cb .gb_Ab:focus:active{background-color:rgba(232,234,237,0.1);border:1px solid #5f6368}.gb_Ub[selected=&quot;true&quot;]{overflow:hidden}.gb_Vb&gt;.gb_Ub[selected=&quot;true&quot;]{background-color:rgba(60,64,67,0.1)}.gb_cb .gb_Vb&gt;.gb_Ub[selected=&quot;true&quot;]{background-color:rgba(255,255,255,0.12)}.gb_Ub[selected=&quot;true&quot;]&gt;.gb_1b{display:block;position:absolute;z-index:2}.gb_1b::-moz-focus-inner{border:0}.gb_1b{background-color:transparent;border:none;-webkit-border-radius:4px;border-radius:4px;-webkit-box-sizing:border-box;box-sizing:border-box;color:#fff;cursor:pointer;display:inline-block;font-family:Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:14px;font-weight:500;letter-spacing:.25px;line-height:16px;margin-bottom:1px;min-height:36px;min-width:86px;outline:none;padding:10px 24px;text-align:center;text-decoration:none;top:16px;width:auto}.gb_2b.gb_3b{background-color:#1a73e8;color:#fff;margin-left:0;margin-right:12px;margin-top:14px}.gb_cb .gb_2b.gb_3b{background-color:#8ab4f8;color:#2d2e30}.gb_2b.gb_4b{background-color:#ffffff;border:1px solid #dadce0;color:#3c4043;margin-left:0;margin-right:0;margin-top:11px}.gb_cb .gb_2b.gb_4b{background-color:rgba(218,220,224,0.01);border:1px solid #5f6368;color:#e8eaed}.gb_1b.gb_4b:hover{background-color:#f7f8f8}.gb_1b.gb_4b:focus,.gb_1b.gb_4b:hover:focus{background-color:#f4f4f4}.gb_1b.gb_4b:active{background-color:#f4f4f4;border:1px solid #5f6368;-webkit-box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 1px 3px 1px rgba(60,64,67,0.15);box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 1px 3px 1px rgba(60,64,67,0.15)}.gb_1b.gb_3b:hover{background-color:#2b7de9;border-color:transparent;-webkit-box-shadow:0 1px 2px 0 rgba(66,133,244,0.3),0 1px 3px 1px rgba(66,133,244,0.15);box-shadow:0 1px 2px 0 rgba(66,133,244,0.3),0 1px 3px 1px rgba(66,133,244,0.15)}.gb_1b.gb_3b:focus,.gb_1b.gb_3b:hover:focus{background-color:#5094ed;border-color:transparent;-webkit-box-shadow:0 1px 2px 0 rgba(66,133,244,0.3),0 1px 3px 1px rgba(66,133,244,0.15);box-shadow:0 1px 2px 0 rgba(66,133,244,0.3),0 1px 3px 1px rgba(66,133,244,0.15)}.gb_1b.gb_3b:active{background-color:#63a0ef;-webkit-box-shadow:0 1px 2px 0 rgba(66,133,244,0.3),0 1px 3px 1px rgba(66,133,244,0.15);box-shadow:0 1px 2px 0 rgba(66,133,244,0.3),0 1px 3px 1px rgba(66,133,244,0.15)}.gb_cb .gb_1b.gb_4b:hover{background-color:rgba(232,234,237,0.04)}.gb_cb .gb_1b.gb_4b:focus,.gb_cb .gb_1b.gb_4b:hover:focus{background-color:rgba(232,234,237,0.12);border:1px solid #e8eaed}.gb_cb .gb_1b.gb_4b:active,.gb_cb .gb_1b.gb_4b:active:focus{background-color:rgba(232,234,237,0.1);border:1px solid #5f6368;-webkit-box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15);box-shadow:0 1px 2px 0 rgba(60,64,67,0.3),0 2px 6px 2px rgba(60,64,67,0.15)}.gb_cb .gb_1b.gb_3b:hover{background-color:#93b9f8;-webkit-box-shadow:0 1px 2px 0 rgba(0,0,0,0.3),0 1px 3px 1px rgba(0,0,0,0.15);box-shadow:0 1px 2px 0 rgba(0,0,0,0.3),0 1px 3px 1px rgba(0,0,0,0.15)}.gb_cb .gb_1b.gb_3b:focus,.gb_cb .gb_1b.gb_3b:hover:focus{background-color:#a5c5f9}.gb_cb .gb_1b.gb_3b:active{background-color:#8ab4f8;-webkit-box-shadow:0 1px 2px 0 rgba(0,0,0,0.3),0 2px 6px 2px rgba(0,0,0,0.15);box-shadow:0 1px 2px 0 rgba(0,0,0,0.3),0 2px 6px 2px rgba(0,0,0,0.15)}.gb_Ub[selected=&quot;true&quot;]&gt;.gb_1b:focus{background-color:rgba(0,0,0,.24);-webkit-border-radius:2px;border-radius:2px;outline:0}.gb_Ub[selected=&quot;true&quot;]&gt;.gb_1b:hover,.gb_Ub[selected=&quot;true&quot;]&gt;.gb_1b:focus:hover{background-color:#565656;-webkit-border-radius:2px;border-radius:2px}.gb_Ub[selected=&quot;true&quot;]&gt;.gb_1b:active{-webkit-border-radius:2px;border-radius:2px;background-color:#212121}.gb_4b{left:0;margin-left:5%}.gb_3b{margin-right:5%;right:0}.gb_Ub:first-child,.gb_5b:first-child+.gb_Ub{border-top:0}.gb_cb .gb_Ub:first-child,.gb_cb .gb_5b:first-child+.gb_Ub{border-top:1px solid transparent}.gb_cb .gb_Ub:first-child:focus:hover,.gb_cb .gb_5b:first-child+.gb_Ub:focus:hover,.gb_cb .gb_Ub:first-child:focus,.gb_cb .gb_5b:first-child+.gb_Ub:focus{border-top:1px solid #e8eaed}.gb_cb .gb_Ub:first-child:active,.gb_cb .gb_5b:first-child+.gb_Ub:active,.gb_cb .gb_Ub:first-child:active:focus,.gb_cb .gb_5b:first-child+.gb_Ub:active:focus{border-top:1px solid transparent}.gb_5b{display:none}.gb_Vb&gt;.gb_Ub.gb_6b{cursor:default;opacity:.38}.gb_Vb&gt;.gb_Ub.gb_6b:hover,.gb_Vb&gt;.gb_Ub.gb_6b:focus,.gb_Vb&gt;.gb_Ub.gb_6b:active{background-color:#fff}.gb_7b{border:none;vertical-align:top;height:32px;width:32px}.gb_Xb{display:inline-block;margin:0 0 0 12px}@media screen and (min-width:361px){.gb_l .gb_Xb{width:244px}}@media screen and (max-width:361px){.gb_l .gb_Xb{width:calc( calc(100vw -  12px *2)  -  12px  -  32px  -  33px *2)}}.gb_9a.gb_ua .gb_Xb{max-width:222px}.gb_8b .gb_Xb{margin-top:9px}.gb_9b{color:#3c4043;font:500 14px/18px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;letter-spacing:.25px;text-overflow:ellipsis;overflow:hidden}.gb_cb .gb_9b{color:#e8eaed}.gb_ac .gb_9b{font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:12px;letter-spacing:normal;line-height:16px}.gb_cb .gb_bc{color:#bdc1c6}.gb_bc{color:#5f6368;display:inline-block;font:400  12px / 16px  Roboto,RobotoDraft,Helvetica,Arial,sans-serif;text-overflow:ellipsis;overflow:hidden}@media screen and (min-width:361px){.gb_l .gb_bc{max-width:244px}}@media screen and (max-width:361px){.gb_l .gb_bc{max-width:calc( calc(100vw -  12px *2)  -  12px  -  32px  -  33px *2)}}@media screen and (min-width:361px){.gb_Xb&gt;.gb_bc.gb_cc{max-width:224px}}@media screen and (max-width:361px){.gb_Xb&gt;.gb_bc.gb_cc{max-width:calc( calc( calc(100vw -  12px *2)  -  12px  -  32px  -  33px *2)  -  20px )}}.gb_ac .gb_bc{margin-top:2px}.gb_dc{color:#5d6369;display:block;float:right;font:italic 400 12px/14px Roboto,RobotoDraft,Helvetica,Arial,sans-serif;padding:3px 0 0 20px;text-align:right;visibility:visible}.gb_cb .gb_dc{color:#9aa0a6}.gb_ec{background-color:transparent;display:none;left:0;overflow-wrap:break-word;position:relative;margin-left:44px;white-space:normal;width:100%;word-wrap:break-word;z-index:1}@media screen and (min-width:361px){.gb_ec{max-width:244px}}@media screen and (max-width:361px){.gb_ec{max-width:calc( calc(100vw -  12px *2)  -  33px *2 -  44px )}}.gb_Ub[selected=&quot;true&quot;]&gt;.gb_ec{display:block}.gb_Vb&gt;.gb_Ub[selected=&quot;true&quot;]{height:auto;min-height:91px}.gb_1b:hover{background-color:rgba(100,100,100,0.4)}.gb_fc{display:block;padding:10px 20px}.gb_Zb{outline:0;padding:14px 41px;width:280px}.gb_Zb:hover{background-color:#f7f8f8}.gb_Zb:focus,.gb_Zb:hover:focus{background-color:#f4f4f4}.gb_Zb:active,.gb_Zb:focus:active{background-color:#e8e8e9}.gb_gc{display:inline-block;vertical-align:middle;height:20px;width:20px}.gb_e .gb_hc::before{left:-244px;top:0}.gb_ic{color:#427fed;display:inline-block;padding:0 25px 0 10px;vertical-align:middle;white-space:normal}.gb_jc{color:#3c4043;font:500 14px/18px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif;padding:0 25px 0 16px;text-align:left}@media screen and (min-width:361px){.gb_jc{width:195px}}@media screen and (max-width:361px){.gb_jc{width:calc( calc( calc(100vw -  12px *2)  -  12px  -  32px  -  33px *2)  -  24px  -  25px )}}.gb_cb .gb_jc{color:#e8eaed}.gb_kc{vertical-align:middle}.gb_lc{-webkit-transform:rotate(180deg);transform:rotate(180deg)}.gb_mc{height:108px;position:absolute;right:-6px;top:-6px;width:108px}.gb_nc{height:88px;position:absolute;right:2px;top:-4px;width:88px}@-webkit-keyframes progressmove{0%{margin-left:-100%}to{margin-left:100%}}@keyframes progressmove{0%{margin-left:-100%}to{margin-left:100%}}.gb_oc.gb_Ja{display:none}.gb_oc{background-color:#d2e3fc;height:3px;overflow:hidden}.gb_cb .gb_oc{background-color:rgba(138,180,248,0.24)}.gb_pc{background-color:#1a73e8;height:100%;width:50%;-webkit-animation:progressmove 1.5s linear 0s infinite;animation:progressmove 1.5s linear 0s infinite}.gb_cb&gt;.gb_pc{background-color:#8ab4f8}.gb_Jb,.gb_qc{height:20px;position:absolute;top:-2px;width:20px}.gb_Ub .gb_Ib{display:inline-block;height:16px;position:relative;width:20px}.gb_Jb{display:inline-block}.gb_Ub[selected=&quot;true&quot;] .gb_Jb{-webkit-transform:rotate(180deg);transform:rotate(180deg)}.gb_Ib{display:none}.gb_rc{margin:0 9px}.gb_Ma.gb_jb{height:80px;width:80px}.gb_sc.gb_Ma{height:32px;width:32px}.gb_Oa.gb_Pa{font:400 50px/ 80px  Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif}.gb_Oa.gb_7b{font:400 22px/34px Google Sans,Roboto,RobotoDraft,Helvetica,Arial,sans-serif}.gb_tc{padding-bottom:2px;position:relative}.gb_sc&gt;.gb_Na{bottom:-2px;left:calc(18px +  2px )}.gb_tc&gt;.gb_Na{bottom:11px;height:24px;left:calc(191px -  33px );width:24px}.gb_uc{color:#5f6368;font-family:Roboto,RobotoDraft,Helvetica,Arial,sans-serif;font-size:14px;line-height:19px;margin-top:4px;text-align:center}.gb_sc{display:inline-block;position:relative}.gb_Xb.gb_vc{margin-left:calc( 12px  -  2px  + 1px)}.gb_9a.gb_l::-webkit-scrollbar{width:16px}.gb_H.gb_l::-webkit-scrollbar{width:16px}.gb_9a.gb_l::-webkit-scrollbar-thumb{background:#dadce0;background-clip:padding-box;border:4px solid transparent;-webkit-border-radius:8px;border-radius:8px;-webkit-box-shadow:none;box-shadow:none}.gb_H.gb_l::-webkit-scrollbar-thumb{background:#dadce0;background-clip:padding-box;border:4px solid transparent;-webkit-border-radius:8px;border-radius:8px;-webkit-box-shadow:none;box-shadow:none;min-height:50px}.gb_cb.gb_l::-webkit-scrollbar-thumb{background-color:#5f6368}.gb_H.gb_l::-webkit-scrollbar-track{background:none;border:none}.gb_9a.gb_l::-webkit-scrollbar-track{background:none;border:none}.gb_H.gb_l::-webkit-scrollbar-track:hover{background:none;border:none}.gb_9a.gb_l::-webkit-scrollbar-track:hover{background:none;border:none}.gb_Ja{display:none!important}.gb_Ka{visibility:hidden}#gb#gb a.gb_f,#gb#gb a.gb_g,#gb#gb span.gb_g{color:rgba(0,0,0,0.87);text-decoration:none}#gb#gb a.gb_g:hover,#gb#gb a.gb_g:focus{opacity:.85;text-decoration:underline}.gb_h.gb_i{display:none;padding-left:15px;vertical-align:middle}.gb_h.gb_i:first-child{padding-left:0}.gb_h .gb_g{display:inline-block;line-height:24px;outline:none;vertical-align:middle}#gb#gb.gb_j a.gb_g,#gb#gb.gb_j span.gb_g,#gb#gb .gb_j a.gb_g,#gb#gb .gb_j span.gb_g{color:#fff}#gb#gb.gb_j span.gb_g,#gb#gb .gb_j span.gb_g{opacity:.7}a.gb_0{border:none;color:#4285f4;cursor:default;font-weight:bold;outline:none;position:relative;text-align:center;text-decoration:none;text-transform:uppercase;white-space:nowrap;-webkit-user-select:none}a.gb_0:hover:after,a.gb_0:focus:after{background-color:rgba(0,0,0,.12);content:'';height:100%;left:0;position:absolute;top:0;width:100%}a.gb_0:hover,a.gb_0:focus{text-decoration:none}a.gb_0:active{background-color:rgba(153,153,153,.4);text-decoration:none}a.gb_1{background-color:#4285f4;color:#fff}a.gb_1:active{background-color:#0043b2}.gb_2{-webkit-box-shadow:0 1px 1px rgba(0,0,0,.16);box-shadow:0 1px 1px rgba(0,0,0,.16)}.gb_0,.gb_1,.gb_3,.gb_4{display:inline-block;line-height:28px;padding:0 12px;-webkit-border-radius:2px;border-radius:2px}.gb_3{background:#f8f8f8;border:1px solid #c6c6c6}.gb_4{background:#f8f8f8}.gb_3,#gb a.gb_3.gb_3,.gb_4{color:#666;cursor:default;text-decoration:none}#gb a.gb_4.gb_4{cursor:default;text-decoration:none}.gb_4{border:1px solid #4285f4;font-weight:bold;outline:none;background:#4285f4;background:-webkit-linear-gradient(top,#4387fd,#4683ea);background:linear-gradient(top,#4387fd,#4683ea);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#4387fd,endColorstr=#4683ea,GradientType=0)}#gb a.gb_4.gb_4{color:#fff}.gb_4:hover{-webkit-box-shadow:0 1px 0 rgba(0,0,0,.15);box-shadow:0 1px 0 rgba(0,0,0,.15)}.gb_4:active{-webkit-box-shadow:inset 0 2px 0 rgba(0,0,0,.15);box-shadow:inset 0 2px 0 rgba(0,0,0,.15);background:#3c78dc;background:-webkit-linear-gradient(top,#3c7ae4,#3f76d3);background:linear-gradient(top,#3c7ae4,#3f76d3);filter:progid:DXImageTransform.Microsoft.gradient(startColorstr=#3c7ae4,endColorstr=#3f76d3,GradientType=0)}.gb_wc{min-width:127px;overflow:hidden;position:relative;z-index:987}.gb_xc{position:absolute;padding:0 20px 0 15px}.gb_yc{display:inline-block;line-height:0;outline:none;vertical-align:middle}.gb_zc .gb_yc{position:relative;top:2px}.gb_yc .gb_Ac,.gb_sa{display:block}.gb_Bc{border:none;display:block;visibility:hidden}.gb_yc .gb_Ac{background-position:0 -35px;height:33px;width:92px}img.gb_va{border:0;vertical-align:middle}.gb_j .gb_yc .gb_Ac{background-position:-296px 0}.gb_E .gb_yc .gb_Ac{background-position:-97px 0;opacity:.54}.gb_3f{display:inline-block;line-height:normal;position:relative;z-index:987}.gb_Kg{color:#000;font:13px/27px Arial,sans-serif;left:0;min-width:1002px;position:absolute;top:0;-webkit-user-select:none;width:100%}.gb_ag{font:13px/27px Arial,sans-serif;position:relative;height:60px;width:100%}.gb_Ra .gb_ag{height:28px}#gba{height:60px}#gba.gb_Ra{height:28px}#gba.gb_Lg{height:90px}#gba.gb_Mg{height:132px}#gba.gb_Lg.gb_Ra{height:58px}.gb_ag&gt;.gb_i{height:60px;line-height:58px;vertical-align:middle}.gb_Ra .gb_ag&gt;.gb_i{height:28px;line-height:26px}.gb_ag::before{background:#e5e5e5;bottom:0;content:'';display:none;height:1px;left:0;position:absolute;right:0}.gb_ag{background:#f1f1f1}.gb_Ng .gb_ag{background:#fff}.gb_Ng .gb_ag::before,.gb_Ra .gb_ag::before{display:none}.gb_E .gb_ag,.gb_j .gb_ag,.gb_Ra .gb_ag{background:transparent}.gb_E .gb_ag::before{background:#e1e1e1;background:rgba(0,0,0,.12)}.gb_j .gb_ag::before{background:#333;background:rgba(255,255,255,.2)}.gb_i{display:inline-block;-webkit-flex:0 0 auto;flex:0 0 auto;-webkit-flex:0 0 main-size;flex:0 0 main-size}.gb_i.gb_Og{float:right;-webkit-order:1;order:1}.gb_Pg{white-space:nowrap}.gb_8f .gb_Pg{display:-webkit-flex;display:flex}.gb_Pg,.gb_i{margin-left:0!important;margin-right:0!important}.gb_Ac{background-image:url('//ssl.gstatic.com/gb/images/i1_1967ca6a.png');-webkit-background-size:528px 68px;background-size:528px 68px}@media (min-resolution:1.25dppx),(-webkit-min-device-pixel-ratio:1.25),(min-device-pixel-ratio:1.25){.gb_Ac{background-image:url('//ssl.gstatic.com/gb/images/i2_2ec824b0.png')}}.gb_1a{min-width:165px;padding-left:30px;padding-right:30px;position:relative;text-align:right;z-index:986;-webkit-align-items:center;align-items:center;-webkit-justify-content:flex-end;justify-content:flex-end;-webkit-user-select:none}.gb_Ra .gb_1a{min-width:0}.gb_1a.gb_i{-webkit-flex:1 1 auto;flex:1 1 auto;-webkit-flex:1 1 main-size;flex:1 1 main-size}.gb_5c{line-height:normal;position:relative;text-align:left}.gb_5c.gb_i,.gb_qe.gb_i,.gb_Sa.gb_i{-webkit-flex:0 1 auto;flex:0 1 auto;-webkit-flex:0 1 main-size;flex:0 1 main-size}.gb_zg,.gb_Ag{display:inline-block;padding:0 0 0 15px;position:relative;vertical-align:middle}.gb_qe{line-height:normal;padding-right:15px}.gb_1a .gb_qe{padding-right:0}.gb_Sa{color:#404040;line-height:30px;min-width:30px;overflow:hidden;vertical-align:middle;text-overflow:ellipsis}#gb.gb_Ra.gb_Ra .gb_re,#gb.gb_Ra.gb_Ra .gb_5c&gt;.gb_Ag .gb_ug{background:none;border:none;color:#36c;cursor:pointer;filter:none;font-size:11px;line-height:26px;padding:0;-webkit-box-shadow:none;box-shadow:none}#gb.gb_Ra.gb_j .gb_re,#gb.gb_Ra.gb_j .gb_5c&gt;.gb_Ag .gb_ug{color:#fff}.gb_Ra .gb_re{text-transform:uppercase}.gb_1a.gb_9f{padding-left:0;padding-right:29px}.gb_1a.gb_Bg{max-width:400px}.gb_Cg{background-clip:content-box;background-origin:content-box;opacity:.27;padding:22px;height:16px;width:16px}.gb_Cg.gb_i{display:none}.gb_Cg:hover,.gb_Cg:focus{opacity:.55}.gb_Dg{background-position:-219px -25px}.gb_Eg{background-position:-194px 0;padding-left:30px;padding-right:14px;position:absolute;right:0;top:0;z-index:990}.gb_Xa:not(.gb_Za) .gb_Eg,.gb_9f .gb_Dg{display:inline-block}.gb_Xa .gb_Dg{padding-left:30px;padding-right:0;width:0}.gb_Xa:not(.gb_Za) .gb_Fg{display:none}.gb_1a.gb_i.gb_9f,.gb_9f:not(.gb_Za) .gb_5c{-webkit-flex:0 0 auto;flex:0 0 auto;-webkit-flex:0 0 main-size;flex:0 0 main-size}.gb_Cg,.gb_9f .gb_qe,.gb_Za .gb_5c{overflow:hidden}.gb_Xa .gb_qe{padding-right:0}.gb_9f .gb_5c{padding:1px 1px 1px 0}.gb_Xa .gb_5c{width:75px}.gb_1a.gb_Hg,.gb_1a.gb_Hg .gb_Dg,.gb_1a.gb_Hg .gb_Dg::before,.gb_1a.gb_Hg .gb_qe,.gb_1a.gb_Hg .gb_5c{-webkit-transition:width .5s ease-in-out,min-width .5s ease-in-out,max-width .5s ease-in-out,padding .5s ease-in-out,left .5s ease-in-out;transition:width .5s ease-in-out,min-width .5s ease-in-out,max-width .5s ease-in-out,padding .5s ease-in-out,left .5s ease-in-out}.gb_8f .gb_1a{min-width:0}.gb_1a.gb_2a,.gb_1a.gb_2a .gb_5c,.gb_1a.gb_Ig,.gb_1a.gb_Ig .gb_5c{min-width:0!important}.gb_1a.gb_2a,.gb_1a.gb_2a .gb_i{-webkit-flex:0 0 auto!important;-webkit-box-flex:0 0 auto!important;-webkit-flex:0 0 auto!important;flex:0 0 auto!important}.gb_1a.gb_2a .gb_Sa{width:30px!important}.gb_Jg{margin-right:32px}.gb_Ka{display:none}.gb_ag ::-webkit-scrollbar{height:15px;width:15px}.gb_ag ::-webkit-scrollbar-button{height:0;width:0}.gb_ag ::-webkit-scrollbar-thumb{background-clip:padding-box;background-color:rgba(0,0,0,.3);border:5px solid transparent;-webkit-border-radius:10px;border-radius:10px;min-height:20px;min-width:20px;height:5px;width:5px}.gb_ag ::-webkit-scrollbar-thumb:hover,.gb_ag ::-webkit-scrollbar-thumb:active{background-color:rgba(0,0,0,.4)}#gb.gb_Qg{min-width:980px}#gb.gb_Qg .gb_7f{min-width:0;position:static;width:0}.gb_dd{display:none}.gb_Qg .gb_ag{background:transparent;border-bottom-color:transparent}.gb_Qg .gb_ag::before{display:none}.gb_Qg.gb_Qg .gb_h{display:inline-block}.gb_Qg.gb_1a .gb_qe{padding-right:15px}.gb_Qg.gb_8f #gbqf{display:block}.gb_Qg #gbq{height:0;position:absolute}.gb_Qg.gb_1a{z-index:987}sentinel{}#gbq .gbgt-hvr,#gbq .gbgt:focus{background-color:transparent;background-image:none}.gbqfh#gbq1{display:none}.gbxx{display:none !important}#gbq{line-height:normal;position:relative;top:0;white-space:nowrap}#gbq{left:0;width:100%}[dir=rtl] #gbq{right:0}#gbq2{top:0;z-index:986}#gbq4{display:inline-block;max-height:29px;overflow:hidden;position:relative}.gbqfh#gbq2{z-index:985}.gbqfh#gbq2{margin:0;margin-left:0 !important;padding-top:0;position:relative;top:310px}.gbqfh #gbqf{margin:auto;min-width:534px;padding:0 !important}.gbqfh #gbqfbw{display:none}.gbqfh #gbqfbwa{display:block}.gbqfh #gbqf{max-width:572px;min-width:572px}.gbqfh .gbqfqw{border-right-width:1px}
@-webkit-keyframes qs-timer {0%{}}
//]]>//
		**/
	}
}
