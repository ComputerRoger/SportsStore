using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GeneralClassLibrary;
using AsyncSockets;

namespace BrowserFormServer
{
	public class ServerProtocolFactory : IProtocolFactory
	{
		public IServerProtocol CreateServerProtocol( Socket clientSocket, ILogger logger )
		{
			ServiceRequest serviceRequest;

			serviceRequest = new ServiceRequest();
			return new BrowserServerProtocol( clientSocket, logger, serviceRequest );
		}
	}
}

