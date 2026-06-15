using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using InputSimulatorPro;
using InputSimulatorPro.Resources.Natives;
using Nefarius.ViGEm;
using Nefarius.ViGEm.Client;
using Nefarius.ViGEm.Client.Targets;
using Nefarius.ViGEm.Client.Targets.Xbox360;
namespace DuoPinballCore9
{
     
    internal class MainPresenter
    {
        private Form1 form;
        string? serialport = null;
        private HighPerformanceSerialReceiver receiver;
        private bool _usexbox360 = false;

        public bool UseXbox360
        {
            get
            {
                return _usexbox360 ;
            }
            set
            {
                _usexbox360 = value;
            }
        }
        private ViGEmClient _vigemclient;
        private ViGEmClient ViGEmClient
        {
            get { if(_vigemclient == null)
                {
                    _vigemclient = new ViGEmClient();
                }
                return _vigemclient;
            }
      
        }
        private IXbox360Controller _xbox360Controller;
        private IXbox360Controller Xbox360Controller
        {
            get
            {
                if(_xbox360Controller == null)
                {

                    this._xbox360Controller = this.ViGEmClient.CreateXbox360Controller();
                    this._xbox360Controller.Connect();
                }
                return this._xbox360Controller;
            }
        }

        private BindingSource _logitems = new BindingSource();

        public BindingSource LogItems
        {
            get
            {
                return _logitems;
            }
            set
            {
                _logitems =  value;
            }
        }

        private VirtualKeyShort[] _flippers = {VirtualKeyShort.LSHIFT, VirtualKeyShort.RSHIFT};
        private VirtualKeyShort[] _rightFlipper = { VirtualKeyShort.RSHIFT };
        private VirtualKeyShort[] _leftFlipper = { VirtualKeyShort.LSHIFT };
        private VirtualKeyShort[] _plunger = { VirtualKeyShort.SPACE };

        internal MainPresenter(Form1 form)
        {
            this.form = form;
        }
        internal string QueryPortName()
        {

            if(String.IsNullOrEmpty(serialport))
            {
                /// UpdateStatus("Searching for Duo Pinball, please wait...");
                serialport = null;
                try
                {
                    this.form.UpdateLog("Querying Duo Pinball. make sure it's connected ");
                    ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT DeviceID,PNPDeviceID,Caption from Win32_SerialPort");
                    foreach(ManagementObject queryObj in searcher.Get())
                    {
                        //  UpdateLog(queryObj["DeviceID"].ToString() + "\t" + queryObj["Caption"].ToString());
                        //Search for the DuoPin VID/PID string 
                        if(queryObj["PNPDeviceID"].ToString().Contains("VID&00010039_PID&5035"))
                        {
                            serialport = queryObj["DeviceID"].ToString();

                            //    UpdateStatus("Paired on " + queryObj["Caption"].ToString());
                            this.form.UpdateLog("Duo Pinball found paired to " + serialport);
                            return serialport;
                            break;
                        }
                    }
                }
                catch(ManagementException error)
                {
                    form.UpdateLog("Unable to read list of COM ports from WMI: " + error.Message);

                }

                return string.Empty;
            }

            return string.Empty;
        }



        internal async Task<HighPerformanceSerialReceiver> Start()
        {
            form.Cursor = Cursors.WaitCursor;
            // QueryPortName();
            if(string.IsNullOrEmpty(serialport))
            {
                QueryPortName();
            }
            if(!string.IsNullOrEmpty(serialport))
            {
                receiver = new HighPerformanceSerialReceiver(serialport, 115200);
                receiver.Start();
                form.Cursor = Cursors.Default;
                var cts = new CancellationTokenSource();
                await foreach(var rawBytes in receiver.ConsumeDataAsync(cts.Token))
                {
                    ProcessPackets(rawBytes);
                }
            }
            else
            {
                form.UpdateLog("unable to find duo controller");
                form.Cursor = Cursors.Default;
            }

            
            return receiver;
        }
        internal void ProcessPackets(byte[]? rawBytes)
        {

            if(rawBytes != null)
            {
                if(rawBytes.Length > 6)
                {
                    Span<byte> spanByte = rawBytes;
                    var part1 = spanByte.Slice(0, 6);
                    var part2 = spanByte.Slice(5);
                    AnalyzePacketData(part1.ToArray());
                    AnalyzePacketData(part2.ToArray());

                }
                else
                {
                    byte[] ByteArray = rawBytes;

                    AnalyzePacketData(ByteArray);
                }
            }
        }

        internal void AnalyzePacketData(byte[] ByteArray)
        {
            // validate bytes
            if(ByteArray.Length > 3 && ByteArray[0] == 90 && ByteArray[1] == 165)
            {
                if(ByteArray[2] == 1)
                {
                    // Flippers 
                    if(ByteArray[3] == 0)
                    {
                        // Flippers Released

                       
                        SendInput(_flippers, false);

                        form.UpdateLog("flipper released");
                    }
                    else if(ByteArray[3] == 1)
                    {
                        // Left Flipper
                       
                        SendInput(_leftFlipper, true); 
                        form.UpdateLog("left flipper pressed");
                    }
                    else if(ByteArray[3] == 2)
                    {
                        // Right Flipper
                      
                        SendInput(_rightFlipper, true);
                        form.UpdateLog("right flipper pressed");
                    }
                    else if(ByteArray[3] == 3)
                    {
                        // Both Flippers
                       
                        SendInput(_flippers, true);


                        form.UpdateLog("both flipper pressed");

                   }
                }
                else if(ByteArray[2] == 2)
                {
                    if(ByteArray[4] == 255)
                    {

                        SendInput(_plunger, false);
                        //InputSimulator.Keyboard.KeyUp(VirtualKeyShort.SPACE);
                        form.UpdateLog("plunger deactivated");
                    }
                    else
                    {
                        
                        SendInput(_plunger, false);
                        //InputSimulator.Keyboard.KeyDown(VirtualKeyShort.SPACE);
                        form.UpdateLog("plunger activated");
                    }
                }
                else
                {
                    form.UpdateLog("Unknown Command:  ignored.");
                }
            }
            else
            {
                form.UpdateLog("Garbage received:  ignored.");
            }
        }

        private  void SendInput(VirtualKeyShort[] keys, bool down)
        {
            if(UseXbox360)
            {
                foreach(var key in keys)
                {
                    if(down)
                    {
                        if(key == VirtualKeyShort.LSHIFT)
                        {

                            this.Xbox360Controller.SetButtonState(Xbox360Button.LeftShoulder, true);
                            //   //InputSimulator.Keyboard.KeyDown(key);
                        }
                        else if(key == VirtualKeyShort.RSHIFT)
                        {
                            this.Xbox360Controller.SetButtonState(Xbox360Button.RightShoulder, true);
                        }
                        else if(key == VirtualKeyShort.SPACE)
                        {
                           // InputSimulator.Keyboard.KeyDown(key);
                            this.Xbox360Controller.SetButtonState(Xbox360Button.A, true);
                        }
                    }
                    else
                    {
                        if(key == VirtualKeyShort.LSHIFT)
                        {

                            this.Xbox360Controller.SetButtonState(Xbox360Button.LeftShoulder, false);
                            //   //InputSimulator.Keyboard.KeyDown(key);
                        }
                        else if(key == VirtualKeyShort.RSHIFT)
                        {
                            this.Xbox360Controller.SetButtonState(Xbox360Button.RightShoulder, false);
                        }
                        else if(key == VirtualKeyShort.SPACE)
                        {
                           // InputSimulator.Keyboard.KeyUp(key);
                            this.Xbox360Controller.SetButtonState(Xbox360Button.A, false);
                        }
                        //InputSimulator.Keyboard.KeyUp(key);
                    }
                }

            }
            else
            {
                foreach(var key in keys)
                {
                    if(down)
                    {
                        InputSimulator.Keyboard.KeyDown(key);
                    }
                    else
                    {
                        InputSimulator.Keyboard.KeyUp(key);
                    }
                }
    
        }
        
        }

        internal  void Dispose()
        {
            if(receiver != null)
            {
               receiver.DisposeAsync();
            }
            if(ViGEmClient != null)
            {
            ViGEmClient.Dispose();
            }

        }
    }
}
