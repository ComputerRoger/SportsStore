using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GeneralClassLibrary
{
	[Serializable]
	public class XmlPageRequestFrame : IpcFrameBase
	{
		public XmlPageRequestFrame( XmlPageRequestBody xmlPageRequestBody ) : 
			base( ServiceActionEnum.XmlPageRequestServiceAction )
		{
			XmlPageRequestBody = xmlPageRequestBody;
		}
		public XmlPageRequestBody XmlPageRequestBody { get; protected set; }
	}

	[Serializable]
	public class XmlPageReplyFrame : IpcFrameBase
	{
		public List<string> ResultList;
		public string ResultText;

		public XmlPageReplyFrame() : this( new List<string>(), "" )
		{
		}
		public XmlPageReplyFrame( string resultText ) : this( new List<string>(), resultText )
		{
		}
		public XmlPageReplyFrame( List<string> textList ) : this( textList, "" )
		{
		}
		public XmlPageReplyFrame( List<string> resultList, string resultText ) : base( ServiceActionEnum.XmlPageRequestServiceAction )
		{
			ResultList = resultList;
			ResultText = resultText;
		}
	}
}
//[Serializable]
//public class GetWebPageIpc : ByteArrayBase
//{
//	public GetWebPageIpc( QueryBody postQueryBody ) : base( BrowserIpcEnum.GetWebPage )
//	{
//		PostQueryBody = postQueryBody;
//	}

//	public QueryBody PostQueryBody { get; protected set; }
//}

//[Serializable]
//public class ResultWebPageIpc : ByteArrayBase
//{
//	public ResultWebPageIpc( ResultQueryBody resultQueryBody ) : base( ServiceActionEnum.XmlPageRequestServiceAction )
//	{
//		ResultQueryBody = resultQueryBody;
//	}

//	public ResultQueryBody ResultQueryBody { get; protected set; }
//}