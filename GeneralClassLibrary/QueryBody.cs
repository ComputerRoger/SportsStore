using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralClassLibrary
{
	[Serializable]
	public class QueryBody
	{
		public string ReceiveUrl;
		public string ReceiveSearch;

		public QueryBody()
		{
			ReceiveUrl = "";
			ReceiveSearch = "";
		}
	}


	[Serializable]
	public class ResultQueryBody
	{
		public List<string> ResultList;
		public string ResultText;

		public ResultQueryBody() : this( new List<string>(), "" )
		{
		}
		public ResultQueryBody( string resultText ) : this( new List<string>(), resultText )
		{
		}
		public ResultQueryBody( List<string> textList ) : this( textList, "" )
		{
		}
		public ResultQueryBody( List<string> resultList, string resultText )
		{
			ResultList = resultList;
			ResultText = resultText;
		}
	}
}
