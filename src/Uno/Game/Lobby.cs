using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Uno.Game
{
    public partial class Lobby : Form
    {
		#region Properties
		public readonly GameConnection gameCon;
		static bool IsAdmin { get { return GameHost.IsHosting; } }

		#endregion

		#region Init / Constructor
		public static Lobby CreateNewGame(string nickName, GameHostFactory ghf)
		{
			if (GameHost.IsHosting)
				throw new InvalidOperationException ("Can't create an other game while hosting one!");

			var host = GameHost.HostGame (ghf);

			if (!GameHost.IsHosting)
				return null;

			var lobby = TryJoinGame (host.Address, host.Id, nickName, ghf);

			if (lobby == null)
				host.Shutdown ();

			return lobby;
		}

		public static Lobby TryJoinGame(IPEndPoint ip, long hostId, string nickName, GameHostFactory ghf)
		{
			// Connect to server, ask if players are available (indirectly - you'll become kicked/rejected otherwise)
			// If connect happened successfully and connection is established, show lobby
			// -- Always expect to be kicked/disconnected for no obvious reasons!
			var connection = GameConnection.TryEstablishConnection (ip, hostId, nickName, ghf);

			if (connection == null)
				return null;

			var lobby = new Lobby(connection);

			lobby.Show();
			return lobby;
		}

		Lobby(GameConnection gameCon)
        {
			this.gameCon = gameCon;
            InitializeComponent();
        }

        private void Lobby_Load(object sender, EventArgs e)
        {

        }
		#endregion

		private void button_ReturnToLobby_Click(object sender, EventArgs e)
		{
			GameHost.ShutDown ();
			Close();
		}
    }
}
