using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Windows.Forms;



namespace DuoPinballUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string serialport = null;
        int plungerval = 0;
        SerialPort mySerialPort;
        //private InputSimulator _inputSimulator;
        //public InputSimulator inputSimulator
        //{
        //    get
        //    {
        //        //if(_inputSimulator == null)
        //        {
        //            //_inputSimulator = new InputSimulator();
        //        }
        //        //return _inputSimulator;

        //    }
        //    private set
        //    {
        //        //_inputSimulator = value;
        //    }
        //}



        public void Form1_Load(object sender, EventArgs e)
        {
        }

        public void ConnectSerial()
        {
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
                        //    UpdateStatus("Paired on " + queryObj["Caption"].ToString());
                         //   UpdateLog("Duo Pinball found paired to " + serialport);
                            break;
                        }
                    }
                }
                catch(ManagementException error)
                {
                 //   UpdateLog("Unable to read list of COM ports from WMI: " + error.Message);
                }
            }

            if(String.IsNullOrEmpty(serialport))
            {
             //   UpdateLog("Unable to find Duo Pinball COM port, please ensure it is paired with Bluetooth before launching this application.");
            }
            else
            {
                 mySerialPort = new SerialPort(serialport);
                mySerialPort.BaudRate = 9600;
                mySerialPort.Parity = Parity.None;
                mySerialPort.StopBits = StopBits.One;
                mySerialPort.DataBits = 8;
                mySerialPort.Handshake = Handshake.None;
                mySerialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                try
                {
                  //  UpdateLog("Opening serial port " + serialport);
                    mySerialPort.Open();
                  //  ButtonText("Reconnect");
                  //  UpdateLog("Duo Pinball should now be working, you can see if the buttons are working or not above.\rIn games the flippers are mapped to left and right shift, the plunger button is mapped to Enter.  Analog plunger is not (yet) supported.");
                }
                catch
                {
                   // UpdateLog("Unable to open serial port, please ensure Duo Pinball is awake and in range and try again.\rIf this happens and it took a long time to find the Duo Pinball COM port it often means the controller has locked up, try removing and re-inserting the batteries.");
                  //  ButtonText("Retry");
                }
            }
        }

        public void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            char[] CharArray = indata.ToCharArray();
            byte[] ByteArray = new byte[CharArray.Length];
            // Convert ASCII string to HEX
            for(int i = 0; i < CharArray.Length; i++)
            {
                ByteArray[i] = Convert.ToByte(CharArray[i]);
                ByteArray[i] += 0x30;
            }
            string outdata = "";
            for(int i = 0; i < CharArray.Length; i++)
            {
                outdata += ByteArray[i] + " ";
            }

            // If the first two digits are correct then interpret the command
            if(ByteArray.Length > 3 && ByteArray[0] == 138 && ByteArray[1] == 111)
            {
                if(ByteArray[2] == 49)
                {
                    // Flippers
                    if(ByteArray[3] == 48)
                    {
                        // Flippers Released


                      
                          //inputSimulator.Keyboard.KeyUp(VirtualKeyCode.LSHIFT);
                        //inputSimulator.Keyboard.KeyUp(VirtualKeyCode.RSHIFT);
                        //LeftFlipper(false);
                        //RightFlipper(false);
                        UpdateLog("flipper released");
                    }
                    else if(ByteArray[3] == 49)
                    {
                        // Left Flipper
                        //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                       
                        //LeftFlipper(true);
                        //RightFlipper(false);
                        UpdateLog("left flipper pressed");
                    }
                    else if(ByteArray[3] == 50)
                    {
                        // Right Flipper
                      //  InputSimulator.SimulateKeyUp(VirtualKeyCode.LSHIFT);
                        //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RSHIFT);
                        //LeftFlipper(false);
                        //RightFlipper(true);
                        UpdateLog("right flipper pressed");
                    }
                    else if(ByteArray[3] == 51)
                    {
                        // Both Flippers
                         //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.LSHIFT);
                        //inputSimulator.Keyboard.KeyDown(VirtualKeyCode.RSHIFT);
                        //LeftFlipper(true);
                        //RightFlipper(true);
                        UpdateLog("both flipper pressed");

                    }
                }
                else if(ByteArray[2] == 50)
                {
                    if(ByteArray[5] == 49)
                    {
                        // Ball Launcher Button
                       // Plunger(0);
                        if(plungerval != 0)
                        {
                            plungerval = 0;
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
                    UpdateLog("Unknown Command: " + outdata + " ignored.");
                }
            }
            else
            {
               UpdateLog("Garbage received: " + outdata + " ignored.");
            }
        }


// TODO: close serial cnnection and dispose

        public delegate void ControlStringConsumer( string text);  // defines a delegate type

        public void UpdateLog(string v)
        {
           string updated = this.textBox1.Text + System.Environment.NewLine + v;
            if(this.textBox1.InvokeRequired)
            {
                this.textBox1.Invoke(new ControlStringConsumer(UpdateLog), new object[] {  updated });  // invoking itself
            }
            else
            {
                this.textBox1.Text = updated;      // the "functional part", executing only on the main thread
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConnectSerial();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(mySerialPort != null)
            {
            
            mySerialPort.Close();
            mySerialPort.Dispose();
            
            
            mySerialPort = null;
            }
        }
    }
}
