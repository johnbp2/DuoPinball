namespace DuoPinballCore9
{
    partial class Form1
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
            if(disposing && (components != null))
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnConnect = new Button();
            this.label1 = new Label();
            this.btnDisconnect = new Button();
            this.cbXbox360 = new CheckBox();
            this.dataGridView1 = new DataGridView();
            this.menuStrip1 = new MenuStrip();
            this.aboutToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new Point(661, 455);
            this.btnConnect.Margin = new Padding(4, 3, 4, 3);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new Size(88, 27);
            this.btnConnect.TabIndex = 0;
            this.btnConnect.Text = "connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += this.btnConnect_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(360, 57);
            this.label1.Name = "label1";
            this.label1.Size = new Size(27, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Enabled = false;
            this.btnDisconnect.Location = new Point(661, 488);
            this.btnDisconnect.Name = "btnDisconnect";
            this.btnDisconnect.Size = new Size(88, 23);
            this.btnDisconnect.TabIndex = 3;
            this.btnDisconnect.Text = "disconnect";
            this.btnDisconnect.UseVisualStyleBackColor = true;
            this.btnDisconnect.Click += this.btnDisconnect_Click;
            // 
            // cbXbox360
            // 
            this.cbXbox360.AutoSize = true;
            this.cbXbox360.Location = new Point(661, 430);
            this.cbXbox360.Name = "cbXbox360";
            this.cbXbox360.Size = new Size(89, 19);
            this.cbXbox360.TabIndex = 4;
            this.cbXbox360.Text = "UseXbox360";
            this.cbXbox360.UseVisualStyleBackColor = true;
            this.cbXbox360.CheckedChanged += this.cbXbox360_CheckedChanged;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(13, 82);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new Size(736, 336);
            this.dataGridView1.TabIndex = 7;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.aboutToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new Size(762, 24);
            this.menuStrip1.TabIndex = 8;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += this.aboutToolStripMenuItem_Click;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(762, 637);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.cbXbox360);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.menuStrip1);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "DuoPinball High Performance";
            this.FormClosing += this.Form1_FormClosing;
            this.Load += this.Form1_Load;
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button btnConnect;
        private Label label1;
        private Button btnDisconnect;
        private CheckBox cbXbox360;
        private DataGridView dataGridView1;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem aboutToolStripMenuItem;
    }
}
