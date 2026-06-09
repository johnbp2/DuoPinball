using System.IO.Ports;


using InputSimulatorPro;
using InputSimulatorPro.Resources.Natives;
using System.Management;
using System.Collections.Specialized;
using System.Collections;


namespace DuoPinballCore9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            presenter = new MainPresenter(this);

        }


        int plungerval = 0;
        public delegate void ControlStringConsumer(string text);  // defines a delegate type
        private MainPresenter presenter;







        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cbXbox360.Enabled = false;
            presenter.Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            presenter.Dispose();
        }
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            presenter.Dispose();
            cbXbox360.Enabled = true;
        }








        public void UpdateLog(string v)
        {
            string updated = this.textBox1.Text + System.Environment.NewLine + DateTime.Now.ToString() + " - " + v;

            if(textBox1.InvokeRequired)
            {
                this.textBox1.Invoke(new ControlStringConsumer(UpdateLog), new object[] { updated });  // invoking itself
            }
            else
            {
                this.textBox1.Text = updated;      // the "functional part", executing only on the main thread
            }
        }

        private void cbXbox360_CheckedChanged(object sender, EventArgs e)
        {
            presenter.UseXbox360 = cbXbox360.Checked;
        }
    }
}
