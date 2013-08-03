using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Uno.Game
{
	public abstract class GameConnection
	{
		#region Properties
		UdpClient udp;

		#endregion

		#region Init / Constructor
		public static GameConnection TryEstablishConnection(IPEndPoint ip, GameHostFactory fact)
		{
			var udp = new UdpClient();

			return fact.CreateConnection();
		}

		public GameConnection()
		{

		}
		#endregion
	}
}
