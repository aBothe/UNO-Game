namespace Uno
{
    partial class Lobby
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BnReady = new System.Windows.Forms.Button();
            this.BnStart = new System.Windows.Forms.Button();
            this.lnName = new System.Windows.Forms.Label();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.listView1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 32);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70.56604F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 29.43396F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(371, 265);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BnStart);
            this.panel1.Controls.Add(this.BnReady);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 190);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(365, 72);
            this.panel1.TabIndex = 1;
            // 
            // BnReady
            // 
            this.BnReady.Location = new System.Drawing.Point(45, 26);
            this.BnReady.Name = "BnReady";
            this.BnReady.Size = new System.Drawing.Size(75, 23);
            this.BnReady.TabIndex = 0;
            this.BnReady.Text = "Bereit";
            this.BnReady.UseVisualStyleBackColor = true;
            // 
            // BnStart
            // 
            this.BnStart.Location = new System.Drawing.Point(255, 26);
            this.BnStart.Name = "BnStart";
            this.BnStart.Size = new System.Drawing.Size(75, 23);
            this.BnStart.TabIndex = 1;
            this.BnStart.Text = "Beginnen";
            this.BnStart.UseVisualStyleBackColor = true;
            // 
            // lnName
            // 
            this.lnName.AutoSize = true;
            this.lnName.Location = new System.Drawing.Point(147, 16);
            this.lnName.Name = "lnName";
            this.lnName.Size = new System.Drawing.Size(88, 13);
            this.lnName.TabIndex = 1;
            this.lnName.Text = "Name der Spieler";
            // 
            // listView1
            // 
            this.listView1.Location = new System.Drawing.Point(3, 3);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(121, 97);
            this.listView1.TabIndex = 2;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 299);
            this.Controls.Add(this.lnName);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Lobby";
            this.Text = "Lobby";
            this.Load += new System.EventHandler(this.Lobby_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button BnStart;
        private System.Windows.Forms.Button BnReady;
        private System.Windows.Forms.Label lnName;
        private System.Windows.Forms.ListView listView1;
    }
}