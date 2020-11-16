using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeneralClassLibrary;
using AsyncSockets;

namespace BrowserFormServer
{
	public class ServiceRequest : IServiceRequest
	{
		public IPerformService CreateService( ITcpFrame requestFrame, ILogger logger )
		{
			IPerformService performService;
			IpcFrameBase ipcFrameBase = ( IpcFrameBase ) IpcFrameBase.ByteArrayToObject( requestFrame.FramePacket );

			switch( ipcFrameBase.ServiceActionEnum )
			{
			case ServiceActionEnum.XmlPageRequestServiceAction:
				XmlPageRequestFrame xmlPageRequestFrame = ( XmlPageRequestFrame ) ipcFrameBase;
				XmlPageService xmlPageService = new XmlPageService( xmlPageRequestFrame );
				performService = xmlPageService;
				break;
			case ServiceActionEnum.SizeServiceActionEnum:
			default:
				performService = null;
				break;
			}
			return performService;
		}
	}
}
