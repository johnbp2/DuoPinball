using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace DuoPinballUI
{

    public class HighPerformanceReceiver : IDisposable
    {
        private readonly SerialPort _serialPort;
        private Stream _baseStream;
        private CancellationTokenSource _cts;
        private Task _readTask;

        public HighPerformanceReceiver(string portName, int baudRate = 921600)
        {
            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One)
            {
                // Allocate massive internal buffer limits to prevent driver-level overflow
                ReadBufferSize = 65536,
                WriteBufferSize = 65536,
                ReadTimeout = 500
            };
        }

        public void Start()
        {
            if(_serialPort.IsOpen)
                return;

            _serialPort.Open();
            // Extract the raw stream to bypass SerialPort class overhead
            _baseStream = _serialPort.BaseStream;

            _cts = new CancellationTokenSource();
            _readTask = Task.Factory.StartNew(
                () => ProcessIncomingDataAsync(_cts.Token),
                TaskCreationOptions.LongRunning
            );
        }

        private async Task ProcessIncomingDataAsync(CancellationToken token)
        {
            // Allocate a reusable memory block on the heap once (Zero-allocation loop)
            byte[] buffer = new byte[4096];
            Memory<byte> memoryBuffer = buffer;

            try
            {
                while(!token.IsCancellationRequested)
                {
                    // Non-allocating streaming directly to our pre-allocated memory chunk
                    int bytesRead = await _baseStream.ReadAsync(memoryBuffer, token).ConfigureAwait(false);

                    if(bytesRead > 0)
                    {
                        // High performance processing using Span
                        ReadOnlySpan<byte> dataSpan = memoryBuffer.Span[..bytesRead];
                        ParseProtocol(dataSpan);
                    }
                }
            }
            catch(OperationCanceledException) { /* Normal termination */ }
            catch(Exception ex)
            {
                Console.WriteLine($"Stream Error: {ex.Message}");
            }
        }

        private void ParseProtocol(ReadOnlySpan<byte> rawData)
        {
            // CRITICAL: Perform your packet stitching or data parsing here.
            // Do NOT convert to string unless absolutely necessary.
            // Use raw span parsing for maximum performance.
            for(int i = 0; i < rawData.Length; i++)
            {
                byte b = rawData[i];
                // Process individual packet structures
            }
        }

        public void Dispose()
        {
            _cts?.Cancel();
            try
            {
                _readTask?.Wait(1000);
            }
            catch { }

            _baseStream?.Dispose();
            if(_serialPort != null && _serialPort.IsOpen)
            {
                _serialPort.Close();
            }
            _serialPort?.Dispose();
            _cts?.Dispose();
        }
    }
}