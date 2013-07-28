using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Uno.Game;

namespace Uno.GameList
{
	public partial class ServerListe : Form
	{
		#region Properties
		public readonly ServerListBackend ListBackend;

		public string NickName
		{
			get { return text_Nick.Text; }
			set { text_Nick.Text = value; }
		}

		public bool CanCreateGame
		{
			get
			{
				return !string.IsNullOrEmpty(NickName.Trim());
			}
		}

		public bool CanJoinGame
		{
			get
			{
				return CanCreateGame && list_Servers.SelectedItem != null;
			}
		}
		#endregion

		#region Init / Constructor
		public ServerListe ()
		{
			InitializeComponent ();

			text_Nick_TextChanged(null, null);
			text_Nick.Focus();

			ListBackend = new ServerListBackend ();
			ListBackend.EntryReceived += (obj) => {
				list_Servers.Items.Remove (obj);
				if(obj.State != GameHost.GameState.ShuttingDown)
					list_Servers.Items.Add (obj);
				UpdateButtonStates();
			};
		}

		private void ServerListe_Load(object sender, EventArgs e)
		{
			RefreshServerList();
		}
		#endregion

		#region Button events
		private void Click_JoinGame (object sender, EventArgs e)
		{
			var gho = list_Servers.SelectedItem as GameHostEntry;

			if (gho == null) {
				if(list_Servers.Items.Count < 1)
					MessageBox.Show ("No games available", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
					MessageBox.Show("Please select game", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var lobby = Lobby.TryJoinGame(gho.Address, NickName);

			if (lobby != null)
			{
				lobby.FormClosed += lobby_FormClosed;
				Hide();
			}
		}

		void lobby_FormClosed(object sender, FormClosedEventArgs e)
		{
			Show();
			RefreshServerList();
			UpdateButtonStates();
		}

		private void Click_CreateGame (object sender, EventArgs e)
		{
			if (GameHost.Host.IsHosting)
			{
				MessageBox.Show("Can't host two games!");
				return;
			}

			var lobby = Lobby.CreateNewGame(NickName);

			if (lobby != null)
			{
				lobby.FormClosed += lobby_FormClosed;
				Hide();
			}
		}

		private void Click_Refresh(object sender, EventArgs e)
		{
			RefreshServerList();
		}
		#endregion

		private void text_Nick_TextChanged(object sender, EventArgs e)
		{
			text_Nick.BackColor = CanCreateGame ? Color.White : Color.Red;
			UpdateButtonStates();
		}

		public void RefreshServerList()
		{
			list_Servers.Items.Clear();
			ListBackend.SendExistenceRequest();
		}

		public void UpdateButtonStates()
		{
			button_Create.Enabled = CanCreateGame;
			button_Join.Enabled = CanJoinGame;
		}
	}
}
