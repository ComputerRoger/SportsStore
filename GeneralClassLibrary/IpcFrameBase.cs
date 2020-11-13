using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Runtime.CompilerServices;

namespace GeneralClassLibrary
{
	[Serializable]
	public class IpcFrameBase
	{
		public ServiceActionEnum ServiceActionEnum { get; protected set; }

		public IpcFrameBase()
		{
			this.ServiceActionEnum = ServiceActionEnum.SizeServiceActionEnum;
		}
		public IpcFrameBase( ServiceActionEnum serviceActionEnum )
		{
			this.ServiceActionEnum = serviceActionEnum;
		}

		public byte[] ToByteArray()
		{
			byte[] thisByteArray = IpcFrameBase.ObjectToByteArray( this );
			return thisByteArray;
		}
		public static IpcFrameBase FromByteArray( byte[] byteArray )
		{
			IpcFrameBase ipcFrameBase = ( IpcFrameBase ) IpcFrameBase.ByteArrayToObject( byteArray );
			return ipcFrameBase;
		}

		public bool TestByteArray( ILogger logger )
		{
			byte[] bytes0 = ObjectToByteArray( this );
			object object1 = ByteArrayToObject( bytes0 );
			byte[] bytes1 = ObjectToByteArray( object1 );
			int count0 = bytes0.Length;
			int count1 = bytes1.Length;
			bool isPass = count0 == count1;
			isPass &= count0 > 0;
			for( int index = 0; index < count0; index++ )
			{
				isPass &= bytes0[ index ] == bytes1[ index ];
			}

			logger.WriteEntry( "IsPass = " + isPass.ToString() );
			logger.WriteEntry( "count = " + count0.ToString() );
			return ( isPass );
		}

		// Convert an object to a byte array
		public static byte[] ObjectToByteArray( Object obj )
		{
			if( obj == null )
				return null;

			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			bf.Serialize( ms, obj );

			return ms.ToArray();
		}

		// Convert a byte array to an Object
		public static Object ByteArrayToObject( byte[] arrBytes )
		{
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write( arrBytes, 0, arrBytes.Length );
			memStream.Seek( 0, SeekOrigin.Begin );
			Object obj = ( Object ) binForm.Deserialize( memStream );

			return obj;
		}
	}
}
