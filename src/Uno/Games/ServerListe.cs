using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Uno.Game;
using Uno.Uno;

namespace Uno.Games
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
				return CanCreateGame && list_Servers.SelectedIndex >= 0;
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
		}

		private void ServerListe_Load(object sender, EventArgs e)
		{
			ListBackend.EntryReceived += (obj) =>
			{
				BeginInvoke(new MethodInvoker(() =>
				{
					var items = list_Servers.Items;
					lock (items)
					{
						var i = items.IndexOf(obj);
						if (i >= 0)
							items.RemoveAt(i);

						if (obj.State != GameState.ShuttingDown)
						{
							if (i == -1)
								items.Add(obj);
							else
								items.Insert(i, obj);
						}
					}
					UpdateButtonStates();
				}));
			};

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

			var lobby = Lobby.TryJoinGame(gho.Address.Address, gho.HostId, NickName, UnoHostFactory.Instance);

			lobby_PostCreation (ref lobby);
		}

		void lobby_FormClosed(object sender, FormClosedEventArgs e)
		{
			Show();
			RefreshServerList();
			UpdateButtonStates();
		}

		void lobby_PostCreation(ref Lobby lobby)
		{
			if (lobby != null)
			{
				if (!lobby.ConnectedEvent.WaitOne (5000)) {

					// Just in case it couldn't connect to the own server, shut it down for consistency reasons
					GameHost.ShutDown ();

					MessageBox.Show ("Couldn't connect to server!", "Connection timeout", MessageBoxButtons.OK, MessageBoxIcon.Error);
					lobby.Close ();
					lobby.Dispose ();
					return;
				}

				lobby.FormClosed += lobby_FormClosed;
				Hide();
			}
		}

		private void Click_CreateGame (object sender, EventArgs e)
		{
			if (GameHost.IsHosting)
			{
				MessageBox.Show("Can't host two games!");
				return;
			}

			//TODO: Auswahl an verfügbaren Game-Hosts erschaffen
			var lobby = Lobby.CreateNewGame(NickName, UnoHostFactory.Instance);

			lobby_PostCreation (ref lobby);
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

		void list_Servers_Select(object sender, EventArgs e){
			UpdateButtonStates ();
		}

		public void RefreshServerList()
		{
			lock(list_Servers.Items)
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
