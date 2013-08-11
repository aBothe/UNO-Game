using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Uno.Uno
{
    public partial class ColorChooser : Form
    {
        public CardColor selectedColor;

        public ColorChooser()
        {
            InitializeComponent();
        }

        private void ColorChooser_Load(object sender, EventArgs e)
        {
            tableLayoutPanel1.Dock = DockStyle.Fill;
            Bn_Red.Dock = DockStyle.Fill;
            Bn_Yellow.Dock = DockStyle.Fill;
            Bn_Blue.Dock = DockStyle.Fill;
            Bn_Green.Dock = DockStyle.Fill;

            Bn_Red.BackColor = Color.Red;
            Bn_Yellow.BackColor = Color.Yellow;
            Bn_Blue.BackColor = Color.Blue;
            Bn_Green.BackColor = Color.Green;
        }

        private void Bn_Red_Click(object sender, EventArgs e)
        {
            selectedColor = CardColor.Red;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Bn_Blue_Click(object sender, EventArgs e)
        {
            selectedColor = CardColor.Blue;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Bn_Green_Click(object sender, EventArgs e)
        {
            selectedColor = CardColor.Green;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void Bn_Yellow_Click(object sender, EventArgs e)
        {
            selectedColor = CardColor.Yellow;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
