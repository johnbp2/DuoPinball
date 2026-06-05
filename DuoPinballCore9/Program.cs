

namespace DuoPinballCore9
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static async Task Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
             Application.Run(new Form1());
            //await using var receiver = new HighPerformanceSerialReceiver("COM6", 115200);
            //receiver.Start();

            //var cts = new CancellationTokenSource();

            //// This loop processes data on an entirely separate thread pool allocation
            //Console.WriteLine("Listening to high-speed stream...");
            //await foreach(var rawBytes in receiver.ConsumeDataAsync(cts.Token))
            //{
            //    // Parse binary telemetry frames or strings here without bottlenecking the port!
            //    // Example: ProcessPacket(rawBytes);
            //}
        }
    }
}