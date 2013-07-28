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
		public readonly ServerListBackend ListBackend;

		public ServerListe ()
		{
			InitializeComponent ();

			ListBackend = new ServerListBackend ();
			ListBackend.EntryReceived += (obj) => {
				lbServer.Items.Remove (obj);
				lbServer.Items.Add (obj);
			};
		}

		private void ServerListe_Load (object sender, EventArgs e)
		{
			tableLayoutPanel1.Dock = DockStyle.Fill;
			lbServer.Dock = DockStyle.Fill;
			pnButton.Dock = DockStyle.Fill;

		}

		private void BnJoin_Click (object sender, EventArgs e)
		{
			var gho = lbServer.SelectedItem as GameHostEntry;
			if (gho == null) {
				MessageBox.Show ("Es sind keine Spiele verfügbar",
				                                "Fehler",
				                                MessageBoxButtons.OK,
				                                MessageBoxIcon.Warning,
				                                MessageBoxDefaultButton.Button3);

			}

			// Connect to server, ask if players are available (indirectly - you'll become kicked/rejected otherwise)
			// If connect happened successfully and connection is established, show lobby
			// -- Always expect to be kicked/disconnected for no obvious reasons!
			MessageBox.Show ("Connect to "+gho.ToString());
		}

		private void BnCreate_Click (object sender, EventArgs e)
		{
			Lobby frame = new Lobby ();
			liste.Add (frame);
			frame.Show ();
		}
	}
}
