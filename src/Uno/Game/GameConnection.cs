using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Uno.Game
{
	public class GameConnection
	{
		#region Properties
		TcpClient tcp;

		#endregion

		#region Init / Constructor
		public GameConnection TryEstablishConnection(IPEndPoint ip)
		{
			var tcp = new TcpClient();
			tcp.Connect(ip);

			tcp.GetStream();

			return new GameConnection();
		}

		private GameConnection()
		{

		}
		#endregion
	}
}
