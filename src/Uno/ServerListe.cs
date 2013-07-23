using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Uno
{
    public partial class ServerListe : Form
    {
        ArrayList liste = new ArrayList();

        public ServerListe()
        {
            InitializeComponent();
        }

        private void ServerListe_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.Dock = DockStyle.Fill;
            lbServer.Dock = DockStyle.Fill;
            pnButton.Dock = DockStyle.Fill;

        }

        private void BnJoin_Click(object sender, EventArgs e)
        {
            if (liste.Count == 0)
            {
                MessageBox.Show("Es sind keine Spiele verfügbar",
                 "Fehler",
                 MessageBoxButtons.OK,
                 MessageBoxIcon.Warning,
                 MessageBoxDefaultButton.Button3);
               
            }
        }

        private void BnCreate_Click(object sender, EventArgs e)
        {
            Lobby frame = new Lobby();
            liste.Add(frame);
            frame.Show();
        }
    }
}
