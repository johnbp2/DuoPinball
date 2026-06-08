using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using InputSimulatorPro;
using InputSimulatorPro.Resources.Natives;

namespace DuoPinballCore9
{
     
    internal class MainPresenter
    {
        private Form1 form;
        string? serialport = null;
        private HighPerformanceSerialReceiver receiver;
        //private InputSimulatorPro.InputSimulator _inputSimulator;
        //internal InputSimulator inputSimulator
        //{
        //    get
        //    {
        //        if(_inputSimulator == null)
        //        {
        //            _inputSimulator = new InputSimulator();
        //        }
        //        return _inputSimulator;

        //    }
        //    private set
        //    {
        //        _inputSimulator = value;
        //    }
        //}

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
            QueryPortName();
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
            {form.Cursor = Cursors.Default;
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
                    SimulateInput(part1.ToArray());
                    SimulateInput(part2.ToArray());

                }
                else
                {
                    byte[] ByteArray = rawBytes;

                    SimulateInput(ByteArray);
                }
            }
        }

        internal void SimulateInput(byte[] ByteArray)
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



                        InputSimulator.Keyboard.KeyUp(VirtualKeyShort.LSHIFT);
                        InputSimulator.Keyboard.KeyUp(VirtualKeyShort.RSHIFT);

                        form.UpdateLog("flipper released");
                    }
                    else if(ByteArray[3] == 1)
                    {
                        // Left Flipper
                        InputSimulator.Keyboard.KeyDown(VirtualKeyShort.LSHIFT);

                        form.UpdateLog("left flipper pressed");
                    }
                    else if(ByteArray[3] == 2)
                    {
                        // Right Flipper
                        InputSimulator.Keyboard.KeyDown(VirtualKeyShort.RSHIFT);
                        form.UpdateLog("right flipper pressed");
                    }
                    else if(ByteArray[3] == 3)
                    {
                        // Both Flippers
                        InputSimulator.Keyboard.KeyDown(VirtualKeyShort.LSHIFT);
                        InputSimulator.Keyboard.KeyDown(VirtualKeyShort.RSHIFT);

                        form.UpdateLog("both flipper pressed");

                    }
                }
                else if(ByteArray[2] == 2)
                {
                    if(ByteArray[4] == 255)
                    {

                        InputSimulator.Keyboard.KeyUp(VirtualKeyShort.SPACE);
                        form.UpdateLog("plunger deactivated");
                    }
                    else
                    {
                        InputSimulator.Keyboard.KeyDown(VirtualKeyShort.SPACE);
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

        internal void Dispose()
        {
            if(receiver != null)
            {
                receiver.DisposeAsync();
            }
        }
    }
}
