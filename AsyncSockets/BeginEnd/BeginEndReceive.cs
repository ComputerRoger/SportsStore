using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using GeneralClassLibrary;

namespace AsyncSockets
{
	//	Early development of asynchronous tcp/ip before async/await.
	public class BeginEndReceive
	{
		// State object for receiving data.  
		protected class CallbackReceiveState
		{
			public int SizeTcpByteArray { get; protected set; }
			public byte[] ReceiveBuffer { get; protected set; }
			public StringBuilder Sb { get; protected set; }

			public CallbackReceiveState()
			{
				InitializeCallbackReceiveState();
			}

			public void InitializeCallbackReceiveState()
			{
				Sb = new StringBuilder();
				SizeTcpByteArray = 65000;
				ReceiveBuffer = new byte[ SizeTcpByteArray ];
			}
		}

		public BeginEndReceive( Socket connectedSocket, ILogger logger )
		{
			ConnectedSocket = connectedSocket;
			Logger = logger;
			InitializeAsyncReceive();
		}

		#region Properties.
		public Socket ConnectedSocket { get; protected set; }
		public ILogger Logger { get; protected set; }

		protected ManualResetEvent ReceiveDoneEvent { get; set; } = new ManualResetEvent( false );
		public string ReceivedText { get; protected set; } = "";
		protected CallbackReceiveState ReceiveState { get; set; }

		#endregion

		protected void InitializeAsyncReceive()
		{
			ReceivedText = "";
			ReceiveDoneEvent.Reset();
			ReceiveState = new CallbackReceiveState();
		}

		private void BeginReceive()
		{
			SocketFlags socketFlags;
			int offset;

			Logger.WriteEntry( "BeginReceive() entry" );
			offset = 0;
			socketFlags = SocketFlags.None;
			ConnectedSocket.BeginReceive( ReceiveState.ReceiveBuffer, offset, ReceiveState.SizeTcpByteArray, socketFlags,
				new AsyncCallback( ReceiveCallback ), ReceiveState );
		}

		public string Receive()
		{
			try
			{
				//	Initialize the receive state.
				InitializeAsyncReceive();

				//	Begin receiving the data from the remote device.
				BeginReceive();

				//	Wait for the final callback signal.
				Logger.WriteEntry( "ReceiveDoneEvent.WaitOne() will wait for signal." );
				ReceiveDoneEvent.WaitOne();
			}
			catch( Exception e )
			{
				Logger.WriteEntry( e.ToString() );
			}
			return ( this.ReceivedText );
		}


		private void ReceiveCallback( IAsyncResult asyncResult )
		{
			try
			{
				// Retrieve the state object and the clientSocket socket from the asynchronous state object.  
				CallbackReceiveState callbackReceiveState = ( CallbackReceiveState ) asyncResult.AsyncState;

				// Read data from the remote device.  
				int bytesRead = ConnectedSocket.EndReceive( asyncResult );

				if( bytesRead > 0 )
				{
					Logger.WriteEntry( "ReceiveCallBack() Received " + bytesRead.ToString() );

					// Accumulate the received text. 
					string receivedText = Encoding.ASCII.GetString( callbackReceiveState.ReceiveBuffer, 0, bytesRead );
					callbackReceiveState.Sb.Append( receivedText );

					// Recursively get the rest of the data.  
					BeginReceive();
				}
				else
				{
					Logger.WriteEntry( "ReceiveCallBack() Received 0 bytes.  Should be done. " );

					//	All the text has arrived.  
					if( callbackReceiveState.Sb.Length > 1 )
					{
						this.ReceivedText = callbackReceiveState.Sb.ToString();
					}
					Logger.WriteEntry( "ReceiveDoneEvent.Set() to signal completion." );
					//	Signal the completion of the message.  
					ReceiveDoneEvent.Set();
				}
			}
			catch( Exception e )
			{
				Logger.WriteEntry( e.ToString() );
			}
		}
	}
}
