using System.IO.Ports;


using InputSimulatorPro;
using InputSimulatorPro.Resources.Natives;
using System.Management;
using System.Collections.Specialized;


namespace DuoPinballCore9
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string serialport = null;
        int plungerval = 0;
        // SerialPort? mySerialPort;
        private DuoPinballCore9.HighPerformanceSerialReceiver hpr = new HighPerformanceSerialReceiver("COM6");
        private InputSimulatorPro.InputSimulator _inputSimulator;
        public InputSimulator inputSimulator
        {
            get
            {
                if(_inputSimulator == null)
                {
                    _inputSimulator = new InputSimulator();
                }
                return _inputSimulator;

            }
            private set
            {
                _inputSimulator = value;
            }
        }



        public void Form1_Load(object sender, EventArgs e)
        {
        }

        public string QueryPortName()
        {
            // await using var receiver = await Start();
            // ResetLog();
            if(String.IsNullOrEmpty(serialport))
            {
                /// UpdateStatus("Searching for Duo Pinball, please wait...");
                serialport = null;
                try
                {
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,PNPDeviceID,Caption from Win32_SerialPort");
                    foreach(ManagementObject queryObj in searcher.Get())
                    {
                        //  UpdateLog(queryObj["DeviceID"].ToString() + "\t" + queryObj["Caption"].ToString());
                        //Search for the DuoPin VID/PID string 
                        if(queryObj["PNPDeviceID"].ToString().Contains("VID&00010039_PID&5035"))
                        {
                            serialport = queryObj["DeviceID"].ToString();
                            return serialport;
                            //    UpdateStatus("Paired on " + queryObj["Caption"].ToString());
                               UpdateLog("Duo Pinball found paired to " + serialport);
                            break;
                        }
                    }
                }
                catch(ManagementException error)
                {
                       UpdateLog("Unable to read list of COM ports from WMI: " + error.Message);

                }

                return string.Empty;
            }

            return string.Empty;
        }

        private HighPerformanceSerialReceiver receiver;

        private async Task<HighPerformanceSerialReceiver> Start()
        {
            this.Cursor = Cursors.WaitCursor;
            QueryPortName();
            if(!string.IsNullOrEmpty(serialport))
            {
                receiver = new HighPerformanceSerialReceiver("COM6", 115200);
                receiver.Start();
                this.Cursor = Cursors.Default;
                var cts = new CancellationTokenSource();
                await foreach(var rawBytes in receiver.ConsumeDataAsync(cts.Token))
                {
                    DataReceivedHandler(rawBytes);
                }
            }
    

            return receiver;
        }

        public void DataReceivedHandler(byte[]? rawBytes)
        {
           
            if(rawBytes != null)
            {
                if(rawBytes.Length > 6)
                {
                    Span<byte> spanByte = rawBytes;
                    var  part1 = spanByte.Slice(0, 6);
                    var part2 = spanByte.Slice(5);
                }
                byte[] ByteArray = rawBytes;
                

                // If the first two digits are correct then interpret the command
                if(ByteArray.Length > 3 && ByteArray[0] == 90 && ByteArray[1] == 165)
                {
                    if(ByteArray[2] == 1)
                    {
                        // Flippers
                        if(ByteArray[3] == 0)
                        {
                            // Flippers Released



                            InputSimulator.Keyboard.KeyUp(VirtualKeyShort.LSHIFT);
                            InputSimulator.Keyboard.KeyUp(VirtualKeyShort.RSHIFT);
                            //LeftFlipper(false);
                            //RightFlipper(false);
                            UpdateLog("flipper released");
                        }
                        else if(ByteArray[3] == 1)
                        {
                            // Left Flipper
                            //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                            InputSimulator.Keyboard.KeyDown(VirtualKeyShort.LSHIFT);
                            //LeftFlipper(true);
                            //RightFlipper(false);
                            UpdateLog("left flipper pressed");
                        }
                        else if(ByteArray[3] == 2)
                        {
                            // Right Flipper
                            InputSimulator.Keyboard.KeyDown(VirtualKeyShort.RSHIFT);
                            //  InputSimulator.SimulateKeyUp(VirtualKeyCode.LSHIFT);
                            //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RSHIFT);
                            //LeftFlipper(false);
                            //RightFlipper(true);
                            UpdateLog("right flipper pressed");
                        }
                        else if(ByteArray[3] == 3)
                        {
                            // Both Flippers
                            InputSimulator.Keyboard.KeyDown(VirtualKeyShort.LSHIFT);
                            InputSimulator.Keyboard.KeyDown(VirtualKeyShort.RSHIFT);
                            //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                            //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RSHIFT);
                            //LeftFlipper(true);
                            //RightFlipper(true);
                            UpdateLog("both flipper pressed");

                        }
                    }
                    else if(ByteArray[2] == 2)
                    {
                        if(ByteArray[4] == 49)
                        {
                            // Ball Launcher Button
                            // Plunger(0);
                            if(plungerval != 0)
                            {
                                plungerval = 0;
                                InputSimulator.Keyboard.KeyPress(VirtualKeyShort.SPACE);
                                //  InputSimulator.SimulateKeyPress(VirtualKeyCode.RETURN);
                            }
                        }
                        else
                        {
                            plungerval = ByteArray[3] - 47;
                            //  Plunger(plungerval);
                        }
                    }
                    else
                    {
                        UpdateLog("Unknown Command:  ignored.");
                    }
                }
                else
                {
                    UpdateLog("Garbage received:  ignored.");
                }
            }
        }


        // TODO: close serial cnnection and dispose

        public delegate void ControlStringConsumer(string text);  // defines a delegate type

        public void UpdateLog(string v)
        {
            string updated = this.textBox1.Text + System.Environment.NewLine + v;
            if(textBox1.InvokeRequired)
            {
                this.textBox1.Invoke(new ControlStringConsumer(UpdateLog), new object[] { updated });  // invoking itself
            }
            else
            {
                this.textBox1.Text = updated;      // the "functional part", executing only on the main thread
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if(mySerialPort != null)
            //{
            if(receiver != null)
            {
            receiver.DisposeAsync();
            }
            //    mySerialPort.Close();
            //    mySerialPort.Dispose();


            //    mySerialPort = null;
            //}
        }
    }
}
