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
	public class ByteArrayBase
	{
		public enum BrowserIpcEnum
		{
			GetWebPage,
			ResultWebPage,
			SIZE_BrowserIpcEnum
		}
		public BrowserIpcEnum BrowserIpc { get; protected set; }

		public ByteArrayBase()
		{
			this.BrowserIpc = BrowserIpcEnum.SIZE_BrowserIpcEnum;
		}
		public ByteArrayBase( BrowserIpcEnum browserIpcEnum )
		{
			this.BrowserIpc = browserIpcEnum;
		}

		public byte[] ToByteArray()
		{
			byte[] thisByteArray = ByteArrayBase.ObjectToByteArray( this );
			return thisByteArray;
		}
		public static ByteArrayBase FromByteArray( byte[] byteArray )
		{
			ByteArrayBase byteArrayBase = ( ByteArrayBase ) ByteArrayBase.ByteArrayToObject( byteArray );
			return byteArrayBase;
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
