namespace Uno.Games
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
			System.Windows.Forms.Label label1;
			System.Windows.Forms.Label label2;
			this.button_Create = new System.Windows.Forms.Button();
			this.button_Join = new System.Windows.Forms.Button();
			this.list_Servers = new System.Windows.Forms.ListBox();
			this.text_Nick = new System.Windows.Forms.TextBox();
			this.button_Refresh = new System.Windows.Forms.Button();
			label1 = new System.Windows.Forms.Label();
			label2 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label1.Location = new System.Drawing.Point(12, 9);
			label1.Name = "label1";
			label1.Size = new System.Drawing.Size(63, 13);
			label1.TabIndex = 2;
			label1.Text = "Nickname";
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			label2.Location = new System.Drawing.Point(12, 48);
			label2.Name = "label2";
			label2.Size = new System.Drawing.Size(101, 13);
			label2.TabIndex = 4;
			label2.Text = "Available Games";
			// 
			// button_Create
			// 
			this.button_Create.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.button_Create.Location = new System.Drawing.Point(211, 294);
			this.button_Create.Name = "button_Create";
			this.button_Create.Size = new System.Drawing.Size(80, 22);
			this.button_Create.TabIndex = 1;
			this.button_Create.Text = "Create Game";
			this.button_Create.UseVisualStyleBackColor = true;
			this.button_Create.Click += new System.EventHandler(this.Click_CreateGame);
			// 
			// button_Join
			// 
			this.button_Join.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
			this.button_Join.Location = new System.Drawing.Point(125, 294);
			this.button_Join.Name = "button_Join";
			this.button_Join.Size = new System.Drawing.Size(80, 22);
			this.button_Join.TabIndex = 0;
			this.button_Join.Text = "Join Game";
			this.button_Join.UseVisualStyleBackColor = true;
			this.button_Join.Click += new System.EventHandler(this.Click_JoinGame);
			// 
			// list_Servers
			// 
			this.list_Servers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.list_Servers.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.list_Servers.FormattingEnabled = true;
			this.list_Servers.Location = new System.Drawing.Point(12, 64);
			this.list_Servers.Name = "list_Servers";
			this.list_Servers.Size = new System.Drawing.Size(305, 223);
			this.list_Servers.TabIndex = 1;
			this.list_Servers.SelectedIndexChanged += list_Servers_Select;
			// 
			// text_Nick
			// 
			this.text_Nick.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.text_Nick.Location = new System.Drawing.Point(12, 25);
			this.text_Nick.Name = "text_Nick";
			this.text_Nick.Size = new System.Drawing.Size(305, 20);
			this.text_Nick.TabIndex = 3;
			this.text_Nick.TextChanged += new System.EventHandler(this.text_Nick_TextChanged);
			// 
			// button_Refresh
			// 
			this.button_Refresh.Location = new System.Drawing.Point(44, 294);
			this.button_Refresh.Name = "button_Refresh";
			this.button_Refresh.Size = new System.Drawing.Size(75, 23);
			this.button_Refresh.TabIndex = 5;
			this.button_Refresh.Text = "Refresh";
			this.button_Refresh.UseVisualStyleBackColor = true;
			this.button_Refresh.Click += new System.EventHandler(this.Click_Refresh);
			// 
			// ServerListe
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(329, 328);
			this.Controls.Add(this.button_Refresh);
			this.Controls.Add(label2);
			this.Controls.Add(this.text_Nick);
			this.Controls.Add(label1);
			this.Controls.Add(this.button_Create);
			this.Controls.Add(this.button_Join);
			this.Controls.Add(this.list_Servers);
			this.DoubleBuffered = true;
			this.MaximizeBox = false;
			this.Name = "ServerListe";
			this.Text = "UNO";
			this.Load += new System.EventHandler(this.ServerListe_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ListBox list_Servers;
        private System.Windows.Forms.Button button_Create;
		private System.Windows.Forms.Button button_Join;
		private System.Windows.Forms.TextBox text_Nick;
		private System.Windows.Forms.Button button_Refresh;
    }
}