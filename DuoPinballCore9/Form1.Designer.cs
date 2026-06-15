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
            this.button1 = new Button();
            this.label1 = new Label();
            this.btnDisconnect = new Button();
            this.cbXbox360 = new CheckBox();
            this.listView1 = new ListView();
            this.dataGridView1 = new DataGridView();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new Point(661, 413);
            this.button1.Margin = new Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(88, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += this.button1_Click;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(13, 9);
            this.label1.Name = "label1";
            this.label1.Size = new Size(27, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Log";
            // 
            // btnDisconnect
            // 
            this.btnDisconnect.Location = new Point(661, 446);
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
            this.cbXbox360.Location = new Point(661, 388);
            this.cbXbox360.Name = "cbXbox360";
            this.cbXbox360.Size = new Size(89, 19);
            this.cbXbox360.TabIndex = 4;
            this.cbXbox360.Text = "UseXbox360";
            this.cbXbox360.UseVisualStyleBackColor = true;
            this.cbXbox360.CheckedChanged += this.cbXbox360_CheckedChanged;
            // 
            // listView1
            // 
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.LabelWrap = false;
            this.listView1.Location = new Point(12, 236);
            this.listView1.Name = "listView1";
            this.listView1.ShowGroups = false;
            this.listView1.Size = new Size(737, 145);
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = View.List;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new Point(13, 46);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new Size(736, 150);
            this.dataGridView1.TabIndex = 7;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(762, 478);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.cbXbox360);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "DuoPinball High Performance";
            this.FormClosing += this.Form1_FormClosing;
            this.Load += this.Form1_Load;
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private Label label1;
        private Button btnDisconnect;
        private CheckBox cbXbox360;
        private ListView listView1;
        private DataGridView dataGridView1;
    }
}
