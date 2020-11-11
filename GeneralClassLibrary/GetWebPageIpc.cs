using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralClassLibrary
{
	[Serializable]
	public class GetWebPageIpc : ByteArrayBase
	{
		public GetWebPageIpc( QueryBody postQueryBody ) : base( BrowserIpcEnum.GetWebPage )
		{
			PostQueryBody = postQueryBody;
		}

		public QueryBody PostQueryBody { get; protected set; }
	}

	[Serializable]
	public class ResultWebPageIpc : ByteArrayBase
	{
		public ResultWebPageIpc( ResultQueryBody resultQueryBody ) : base( BrowserIpcEnum.ResultWebPage )
		{
			ResultQueryBody = resultQueryBody;
		}

		public ResultQueryBody ResultQueryBody { get; protected set; }
	}
}
