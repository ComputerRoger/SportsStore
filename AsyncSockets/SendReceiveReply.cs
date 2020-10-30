using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using GeneralClassLibrary;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq.Expressions;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Net.Configuration;
using System.Net;

namespace AsyncSockets
{
	public class SendReceiveReply
	{
		public SendReceiveReply()
		{
		}

		static public async Task<BufferedStream> ConnectToServer( string serverHostName, int serverPortNumber, int sizeStreamBuffer, ILogger logger )
		{
			string methodName = "ConnectToServer";
			logger.WriteEntry( methodName + " entry." );

			// Establish the remote endpoint for the socket.  
			IPHostEntry ipHostInfo = Dns.GetHostEntry( serverHostName );
			IPAddress ipAddress = ipHostInfo.AddressList[ 0 ];
			IPEndPoint serverEndPoint = new IPEndPoint( ipAddress, serverPortNumber );

			// Create a TCP/IP socket. 
			Socket clientSocket = new Socket( ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp );
			await clientSocket.ConnectAsync( serverHostName, serverPortNumber );

			//	Convert the connected socket to a TcpClient.
			TcpClient tcpClient = new TcpClient();
			tcpClient.Client = clientSocket;

			//	A network stream is used to send and receive data.
			NetworkStream networkStream = tcpClient.GetStream();
			BufferedStream bufferedStream = new BufferedStream( networkStream, sizeStreamBuffer );
			return bufferedStream;
		}

		static public async Task<int> SendFrame( ITcpFrame tcpFrame, BufferedStream bufferedStream, ILogger logger )
		{
			int count;
			int offset;
			string methodName = "SendFrame";

			int sendFrameSize = tcpFrame.FramePacket.Length;
			byte[] sendFrameBytes = tcpFrame.FramePacket;

			//	Encode the packet length into a byte array.
			byte[] sendLengthBytes = BitConverter.GetBytes( sendFrameSize );

			//	Send the packet length to the server.
			offset = 0;
			count = sendLengthBytes.Length;
			await bufferedStream.WriteAsync( sendLengthBytes, offset, count );

			logger.WriteEntry( methodName + " sent frame length " + sendFrameSize.ToString() );

			if( sendFrameSize > 0 )
			{
				//	Send the request frame.
				offset = 0;
				count = sendFrameSize;
				await bufferedStream.WriteAsync( sendFrameBytes, offset, count );

				logger.WriteEntry( methodName + " sent packet to server. " );
			}
			return sendFrameSize;
		}

		static public async Task<ITcpFrame> ReceiveFrame( BufferedStream bufferedStream, ILogger logger )
		{
			int count;
			int numberRead;
			int offset;
			string methodName = "ReceiveFrame";

			logger.WriteEntry( methodName + " creating receive buffer." );

			//	Read the number of bytes of the packet.
			count = sizeof( Int32 );
			byte[] receiveLengthBytes = new byte[ count ];
			offset = 0;
			while( count > 0 )
			{
				numberRead = await bufferedStream.ReadAsync( receiveLengthBytes, offset, count );
				offset += numberRead;
				count -= numberRead;
			}
			logger.WriteEntry( methodName + " received response size." );

			//	Convert the raw bytes to an integer.
			int receiveFrameSize = BitConverter.ToInt32( receiveLengthBytes, 0 );

			//	Receive the packet.
			byte[] receiveFrameBytes = new byte[ receiveFrameSize ];
			if( receiveFrameSize > 0 )
			{
				//	Receive the data frame.
				offset = 0;
				count = receiveFrameSize;
				while( count > 0 )
				{
					numberRead = await bufferedStream.ReadAsync( receiveFrameBytes, offset, count );
					offset += numberRead;
					count -= numberRead;
				}
			}
			RequestResponseFrame tcpFrame = new RequestResponseFrame( receiveFrameBytes );
			return tcpFrame;
		}

		//	Send a request. Receive the response.
		static public async Task<RequestResponseFrame> RequestReceiveResponse( BufferedStream bufferedStream, ITcpFrame sendTcpFrame, ILogger logger )
		{
			int numberSent;

			int sendFrameSize = sendTcpFrame.FramePacket.Length;
			byte[] sendFrameBytes = sendTcpFrame.FramePacket;

			//	Send the request to the server.
			numberSent = await SendFrame( sendTcpFrame, bufferedStream, logger );

			//	Receive the response from the server.
			ITcpFrame tcpFrame;
			tcpFrame = await ReceiveFrame( bufferedStream, logger );

			//	The response is ready to be processed.
			RequestResponseFrame responseFrame = new RequestResponseFrame( tcpFrame.FramePacket );
			return responseFrame;
		}

		//	Receive a request. Service the request. Reply with the result.
		static public async Task<int> ReceiveServiceReply( BufferedStream bufferedStream, IServiceRequest serviceRequest, ILogger logger, IAppDocument appDocument )
		{
			int numberSent;

			//	Receive the request from the client.
			ITcpFrame requestFrame = await ReceiveFrame( bufferedStream, logger );

			//	Service the request.
			ITcpFrame responseFrame = await serviceRequest.ServiceTheRequest( requestFrame, logger, appDocument );

			//	Reply the response to the client.
			numberSent = await SendFrame( responseFrame, bufferedStream, logger );
			return numberSent;
		}
	}

	public class RequestResponseFrame : ITcpFrame
	{
		private byte[] m_FramePacket;

		public RequestResponseFrame( byte[] framePacket )
		{
			m_FramePacket = framePacket;
		}

		int ITcpFrame.FrameSize => m_FramePacket.Length;

		byte[] ITcpFrame.FramePacket => m_FramePacket;
	}

	public interface ITcpFrame
	{
		int FrameSize { get; }
		byte[] FramePacket { get; }
	}

	//	A server business object will process a request frame to produce a response frame.
	public interface IServiceRequest
	{
		Task<ITcpFrame> ServiceTheRequest( ITcpFrame requestFrame, ILogger logger, IAppDocument appDocument );
	}
}
