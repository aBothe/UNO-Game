namespace Uno.Uno
{
    partial class ColorChooser
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
            this.Bn_Red = new System.Windows.Forms.Button();
            this.Bn_Blue = new System.Windows.Forms.Button();
            this.Bn_Green = new System.Windows.Forms.Button();
            this.Bn_Yellow = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Bn_Red, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.Bn_Blue, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.Bn_Green, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.Bn_Yellow, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(38, 72);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // Bn_Red
            // 
            this.Bn_Red.Location = new System.Drawing.Point(3, 3);
            this.Bn_Red.Name = "Bn_Red";
            this.Bn_Red.Size = new System.Drawing.Size(75, 23);
            this.Bn_Red.TabIndex = 0;
            this.Bn_Red.Text = "button1";
            this.Bn_Red.UseVisualStyleBackColor = true;
            this.Bn_Red.Click += new System.EventHandler(this.Bn_Red_Click);
            // 
            // Bn_Blue
            // 
            this.Bn_Blue.Location = new System.Drawing.Point(103, 3);
            this.Bn_Blue.Name = "Bn_Blue";
            this.Bn_Blue.Size = new System.Drawing.Size(75, 23);
            this.Bn_Blue.TabIndex = 1;
            this.Bn_Blue.Text = "button2";
            this.Bn_Blue.UseVisualStyleBackColor = true;
            this.Bn_Blue.Click += new System.EventHandler(this.Bn_Blue_Click);
            // 
            // Bn_Green
            // 
            this.Bn_Green.Location = new System.Drawing.Point(3, 53);
            this.Bn_Green.Name = "Bn_Green";
            this.Bn_Green.Size = new System.Drawing.Size(75, 23);
            this.Bn_Green.TabIndex = 2;
            this.Bn_Green.Text = "button3";
            this.Bn_Green.UseVisualStyleBackColor = true;
            this.Bn_Green.Click += new System.EventHandler(this.Bn_Green_Click);
            // 
            // Bn_Yellow
            // 
            this.Bn_Yellow.Location = new System.Drawing.Point(103, 53);
            this.Bn_Yellow.Name = "Bn_Yellow";
            this.Bn_Yellow.Size = new System.Drawing.Size(75, 23);
            this.Bn_Yellow.TabIndex = 3;
            this.Bn_Yellow.Text = "button4";
            this.Bn_Yellow.UseVisualStyleBackColor = true;
            this.Bn_Yellow.Click += new System.EventHandler(this.Bn_Yellow_Click);
            // 
            // ColorChooser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ColorChooser";
            this.Text = "ColorChooser";
            this.Load += new System.EventHandler(this.ColorChooser_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Bn_Red;
        private System.Windows.Forms.Button Bn_Blue;
        private System.Windows.Forms.Button Bn_Green;
        private System.Windows.Forms.Button Bn_Yellow;
    }
}