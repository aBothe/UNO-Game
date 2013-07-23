namespace Uno
{
    partial class ServerListe
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
            this.lbServer = new System.Windows.Forms.ListBox();
            this.BnJoin = new System.Windows.Forms.Button();
            this.BnCreate = new System.Windows.Forms.Button();
            this.pnButton = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1.SuspendLayout();
            this.pnButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.pnButton, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lbServer, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(99, 30);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.90392F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.09608F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(440, 281);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lbServer
            // 
            this.lbServer.FormattingEnabled = true;
            this.lbServer.Location = new System.Drawing.Point(3, 3);
            this.lbServer.Name = "lbServer";
            this.lbServer.Size = new System.Drawing.Size(120, 95);
            this.lbServer.TabIndex = 1;
            // 
            // BnJoin
            // 
            this.BnJoin.Location = new System.Drawing.Point(61, 32);
            this.BnJoin.Name = "BnJoin";
            this.BnJoin.Size = new System.Drawing.Size(75, 23);
            this.BnJoin.TabIndex = 0;
            this.BnJoin.Text = "Beitreten";
            this.BnJoin.UseVisualStyleBackColor = true;
            this.BnJoin.Click += new System.EventHandler(this.BnJoin_Click);
            // 
            // BnCreate
            // 
            this.BnCreate.Location = new System.Drawing.Point(341, 32);
            this.BnCreate.Name = "BnCreate";
            this.BnCreate.Size = new System.Drawing.Size(75, 23);
            this.BnCreate.TabIndex = 1;
            this.BnCreate.Text = "Erstellen";
            this.BnCreate.UseVisualStyleBackColor = true;
            this.BnCreate.Click += new System.EventHandler(this.BnCreate_Click);
            // 
            // pnButton
            // 
            this.pnButton.Controls.Add(this.BnCreate);
            this.pnButton.Controls.Add(this.BnJoin);
            this.pnButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnButton.Location = new System.Drawing.Point(3, 191);
            this.pnButton.Name = "pnButton";
            this.pnButton.Size = new System.Drawing.Size(434, 87);
            this.pnButton.TabIndex = 0;
            // 
            // ServerListe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(530, 361);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ServerListe";
            this.Text = "ServerListe";
            this.Load += new System.EventHandler(this.ServerListe_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.pnButton.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.ListBox lbServer;
        private System.Windows.Forms.Panel pnButton;
        private System.Windows.Forms.Button BnCreate;
        private System.Windows.Forms.Button BnJoin;
    }
}