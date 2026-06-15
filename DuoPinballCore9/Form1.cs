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
            this.dataGridView1.AutoGenerateColumns = false;
            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Time";
            column.Name = "Time";
            column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column);
            DataGridViewColumn column2 = new DataGridViewTextBoxColumn();
            column2.DataPropertyName = "Detail";
            column2.Name = "Detail";

            column2.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns.Add(column2);
            this.dataGridView1.DataSource = this.presenter.LogItems;
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
        private async void btnDisconnect_Click(object sender, EventArgs e)
        {
            presenter.Dispose();
            cbXbox360.Enabled = true;
        }








        //public void UpdateLog(string v)
        //{
        //    string updated = this.textBox1.Text + System.Environment.NewLine + DateTime.Now.ToString() + " - " + v;

        //    if(textBox1.InvokeRequired)
        //    {
        //        this.textBox1.Invoke(new ControlStringConsumer(UpdateLog), new object[] { updated });  // invoking itself
        //    }
        //    else
        //    {
        //        this.textBox1.Text = updated;      // the "functional part", executing only on the main thread
        //    }
        //    UpdateLogList(v);
        //}

        public void UpdateLog(string v)
        {
            string updated = DateTime.Now.ToString() + " - " + v;
            this.presenter.LogItems.Add(new LogItem(v));
            if(dataGridView1.Rows.Count > 0)
            {
                int lastRowIndex = dataGridView1.Rows.Count - 1;
                // Set focus to the first visible cell of the last row
                dataGridView1.CurrentCell = dataGridView1.Rows[lastRowIndex].Cells[0];
            }

            if(listView1.InvokeRequired)
            {
                this.listView1.Invoke(new ControlStringConsumer(UpdateLog), new object[] { updated });
            }
            else
            {
                var item = new ListViewItem(updated);

                listView1.Items.Add(item);
            }

        }

        private void cbXbox360_CheckedChanged(object sender, EventArgs e)
        {
            presenter.UseXbox360 = cbXbox360.Checked;
        }
    }


    public class LogItem
    {
        private string detail = "";
        private string time = DateTime.Now.ToString();

        public string Detail
        {
            get => this.detail;
            set => this.detail = value;
        }
        public string Time
        {
            get => this.time;
            set => this.time = value;
        }

        public LogItem(string v)
        {
            Detail = v;
            
        }
    }
}