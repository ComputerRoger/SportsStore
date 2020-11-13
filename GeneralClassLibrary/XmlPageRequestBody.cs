using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralClassLibrary
{
	[Serializable]
	public class XmlPageRequestBody
	{
		public string ReceiveUrl;
		public string ReceiveSearch;

		public XmlPageRequestBody()
		{
			ReceiveUrl = "";
			ReceiveSearch = "";
		}
	}
}
