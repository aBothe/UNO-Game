using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Uno
{
    public partial class Lobby : Form
    {
        public Lobby()
        {
            InitializeComponent();
        }

        private void Lobby_Load(object sender, EventArgs e)
        {
           listView1.Dock = DockStyle.Fill;
            panel1.Dock = DockStyle.Fill;

            setGUI();
        }

        private void setGUI() {

            listView1.View = View.Details;		// Report vs. Details
            listView1.LabelEdit = true;		// EditModus
            listView1.AllowColumnReorder = true;	// Spalten verschieben
            listView1.FullRowSelect = true;		// zeilenweise markieren
            listView1.GridLines = true;		// Gitterlinien
            listView1.CheckBoxes = true;

            listView1.Columns.Add("Bereit", 50, HorizontalAlignment.Left);
            listView1.Columns.Add("IPAdresse", 200, HorizontalAlignment.Left);

            ListViewItem item1 = new ListViewItem();  // 0 = 1. Bild
            item1.Checked = true;
            item1.SubItems.Add("192.168.178.2");	// next Column
            listView1.Items.Add(item1);

        }
    }
}
