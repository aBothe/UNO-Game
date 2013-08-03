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
		List<Player> PlayerList = new List<Player>();
		static bool IsAdmin { get { return GameHost.IsHosting; } }

		#endregion

		#region Init / Constructor
		public static Lobby CreateNewGame(string nickName, GameHostFactory ghf)
		{
			if (GameHost.IsHosting)
				return null;

			var host = GameHost.CreateHost (ghf);

			if (host == null)
				return null;

			var lobby = new Lobby();

			lobby.Show();
			return lobby;
		}

		public static Lobby TryJoinGame(IPEndPoint ip, string nickName)
		{
			if (GameHost.IsHosting)
				return null;

			// Connect to server, ask if players are available (indirectly - you'll become kicked/rejected otherwise)
			// If connect happened successfully and connection is established, show lobby
			// -- Always expect to be kicked/disconnected for no obvious reasons!
			var lobby = new Lobby();

			lobby.Show();
			return lobby;
		}

        Lobby()
        {
            InitializeComponent();
        }

        private void Lobby_Load(object sender, EventArgs e)
        {

        }
		#endregion

		private void button_ReturnToLobby_Click(object sender, EventArgs e)
		{
			Close();
		}
    }
}
