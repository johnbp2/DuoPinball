using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Ports;

namespace DuoPinball
{
    
    internal class Program
    {
        private static SerialPort serialPort;

        public static SerialPort SerialPort
        {
            get
            {
                if(serialPort == null)
                    serialPort = new SerialPort("COM6");
                serialPort.Encoding = Encoding.Unicode;
                return serialPort;
            }

            set => serialPort = value;
        }
        [STAThread]
        static void Main(string[] args)
        {

            //  SerialPort = new SerialPort("COM6");
            SerialPort.DataReceived += SerialPort_DataReceived;
            SerialPort.Open();
            Console.ReadLine();
        }
        private static byte[] buffer;
        private static void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
          //  if(SerialPort.BytesToRead > 0)
          //  { buffer = new byte[SerialPort.BytesToRead];
              
          //var data = SerialPort.ReadExisting();
          //    var chars=  data.ToCharArray();
          //      foreach(char c in chars) {
          //          System.Convert.ToByte(c);
          //      }
              
          //      System.Diagnostics.Debugger.Log(0, "serial", data);
          //  }
            // throw new NotImplementedException();

        }
    }
}
