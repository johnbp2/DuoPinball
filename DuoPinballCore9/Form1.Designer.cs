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
            this.textBox1 = new TextBox();
            this.label1 = new Label();
            this.btnDisconnect = new Button();
            this.cbXbox360 = new CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new Point(607, 401);
            this.button1.Margin = new Padding(4, 3, 4, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(88, 27);
            this.button1.TabIndex = 0;
            this.button1.Text = "connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += this.button1_Click;
            // 
            // textBox1
            // 
            this.textBox1.AcceptsReturn = true;
            this.textBox1.AcceptsTab = true;
            this.textBox1.BorderStyle = BorderStyle.FixedSingle;
            this.textBox1.Location = new Point(13, 46);
            this.textBox1.Margin = new Padding(4, 3, 4, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = ScrollBars.Vertical;
            this.textBox1.Size = new Size(576, 461);
            this.textBox1.TabIndex = 1;
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
            this.btnDisconnect.Location = new Point(607, 434);
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
            this.cbXbox360.Location = new Point(625, 362);
            this.cbXbox360.Name = "cbXbox360";
            this.cbXbox360.Size = new Size(89, 19);
            this.cbXbox360.TabIndex = 4;
            this.cbXbox360.Text = "UseXbox360";
            this.cbXbox360.UseVisualStyleBackColor = true;
            this.cbXbox360.CheckedChanged += this.cbXbox360_CheckedChanged;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(762, 519);
            this.Controls.Add(this.cbXbox360);
            this.Controls.Add(this.btnDisconnect);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Margin = new Padding(4, 3, 4, 3);
            this.Name = "Form1";
            this.Text = "DuoPinball High Performance";
            this.FormClosing += this.Form1_FormClosing;
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private Label label1;
        private Button btnDisconnect;
        private CheckBox cbXbox360;
    }
}
