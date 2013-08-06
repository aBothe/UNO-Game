using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Uno.Game
{
    public partial class Lobby : Form
    {
		#region Properties
		GameConnection Connection;
		public readonly ManualResetEvent ConnectedEvent = new ManualResetEvent (false);
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

			var lobby = TryJoinGame (IPAddress.None, host.Id, nickName, ghf);

			if (lobby == null)
				host.Shutdown ();

			return lobby;
		}

		public static Lobby TryJoinGame(IPAddress ip, long hostId, string nickName, GameHostFactory ghf)
		{
			var lobby = new Lobby();

			// Connect to server, ask if players are available (indirectly - you'll become kicked/rejected otherwise)
			// If connect happened successfully and connection is established, show lobby
			// -- Always expect to be kicked/disconnected for no obvious reasons!
			var con = GameConnection.Create (ip, hostId, ghf);

			con.Connected += lobby.Connected;
			con.Disconnected += lobby.Disconnected;
			con.GeneralPlayerInfoReceived += lobby.PlayerInfoReceived;
			con.OtherPlayerLeft += lobby.OtherPlayerLeft;
			con.ChatArrived += lobby.ChatMessageReceived;
			con.ReadyStateChanged += lobby.ReadStateChanged;

			lobby.Connection = con;

			con.Initialize (nickName);

			return lobby;
		}

		Lobby()
        {
			InitializeComponent();
        }

		~Lobby()
		{
			GameHost.ShutDown ();
		}

        private void Lobby_Load(object sender, EventArgs e)
        {

        }
		#endregion

		#region GameConnection events
		void Connected()
		{
			ConnectedEvent.Set ();
			Program.MainForm.BeginInvoke(new MethodInvoker(Show));

			PlayerInfoReceived (Connection.PlayerNick, false, null);
		}

		void Disconnected(ClientMessage reason, string message)
		{
			ConnectedEvent.Reset ();

			Connection.Dispose ();
			Connection = null;

			Invoke(new MethodInvoker(Close));

			string title;
			switch (reason) {
				default:
					title = "Disconnected";
					break;
				case ClientMessage.JoinDenied:
					title = "Join denied";
					break;
				case ClientMessage.Kicked:
					title = "Kicked";
					break;
				case ClientMessage.Timeout:
					title = "Timeout";
					break;
				case ClientMessage.ServerShutdown:
					title = "Shutdown";
					message = "Server was shut down";
					break;
			}

			MessageBox.Show (message, title, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
		}

		Font boldFont;

		void DrawPlayerItem(object sender, DrawItemEventArgs e)
		{
			e.DrawBackground();

			var pi = list_Players.Items [e.Index] as PlayerInfo;

			var font = e.Font;

			if (pi.isReady) {
				if (boldFont == null)
					boldFont = new Font (e.Font, FontStyle.Bold);

				font = boldFont;
			}

			e.Graphics.DrawString(pi.nick, font, e.State == DrawItemState.Selected ? Brushes.White : Brushes.Black, e.Bounds, StringFormat.GenericDefault);

			e.DrawFocusRectangle();
		}

		class PlayerInfo
		{
			public string nick;
			public bool isReady;

			public override bool Equals (object obj)
			{
				return obj is PlayerInfo && (obj as PlayerInfo).nick == nick;
			}

			public override int GetHashCode ()
			{
				return nick.GetHashCode ();
			}
		}

		void PlayerInfoReceived(string nick, bool isReady, object furtherInfo)
		{
			var pi = new PlayerInfo { nick = nick, isReady = isReady };

			var i = list_Players.Items.IndexOf (pi);

			if (i < 0)
				list_Players.Items.Add (pi);
			else {
				list_Players.Items.RemoveAt (i);
				list_Players.Items.Insert (i, pi);
			}
		}

		void OtherPlayerLeft(string nick)
		{
			var ti = new PlayerInfo { nick = nick };
			list_Players.Items.Remove (ti);
		}

		void ChatMessageReceived(string nick, string message)
		{
			text_ChatLog.BeginInvoke(new MethodInvoker(()=> text_ChatLog.AppendText (nick + ": "+ message + Environment.NewLine)));
		}
		#endregion

		#region Buttons
		void text_ChatMessage_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Return)
				button_SendChat_Click (sender, e);
		}

		void ReadStateChanged(bool ready)
		{
			button_Ready.Checked = ready;
		}

		private void button_Ready_Click(object sender, EventArgs e)
		{
			Connection.IsPlayerReady = button_Ready.Checked;
		}

		private void button_SendChat_Click(object sender, EventArgs e)
		{
			var t = text_ChatMessage.Text;

			if (string.IsNullOrEmpty (t))
				return;

			Connection.SendChat (t);

			text_ChatMessage.Clear ();
		}

		protected override void OnHandleDestroyed (EventArgs e)
		{
			base.OnHandleDestroyed (e);

			GameHost.ShutDown ();
		}

		private void button_ReturnToLobby_Click(object sender, EventArgs e)
		{
			Connection.Disconnect ();
			Close();
		}
		#endregion
    }
}
