using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Threading;
using System.Threading.Channels;    
using System.Threading.Tasks;

namespace DuoPinballCore9
{





    [DebuggerDisplay("{DebugDisplay}")]
    public class HighPerformanceSerialReceiver : IAsyncDisposable
    {
        private readonly SerialPort _serialPort;
        private readonly Channel<byte[]> _packetChannel;
        private CancellationTokenSource _cts;
        private Task _readTask;
        public string DebugDisplay
        {
            get
            {
                return $"SerialPort={this._serialPort}";
            }
        }
        public HighPerformanceSerialReceiver(string portName, int baudRate = 115200)
        {
            // 1. Pre-allocate thread-safe channel for decoupling processing from reading
            _packetChannel = Channel.CreateUnbounded<byte[]>(new UnboundedChannelOptions
            {
                SingleWriter = true, // Performance boost: Only our read loop writes here
                SingleReader = false
            });

            _serialPort = new SerialPort(portName, baudRate)
            {
                Parity = Parity.None,
                DataBits = 8,
                StopBits = StopBits.One,
                Handshake = Handshake.None,

                // Critical tuning: Max out internal buffers to protect against OS context-switches
                ReadBufferSize = 65536,
                WriteBufferSize = 65536
            };
        }

        public void Start()
        {
            if(_serialPort.IsOpen)
                return;

            _serialPort.Open();
            _cts = new CancellationTokenSource();

            // 2. Fire up a dedicated background task to continuously pull hardware data
            _readTask = Task.Run(() => ReadLoopAsync(_cts.Token), _cts.Token);
        }

        private async Task ReadLoopAsync(CancellationToken cancellationToken)
        {
            // Obtain references directly via the OS level stream wrapper
            var stream = _serialPort.BaseStream;
            byte[] buffer = new byte[4096]; // Chunk buffer size matched to typical hardware frames

            try
            {
                while(!cancellationToken.IsCancellationRequested)
                {
                    // 3. Perform a high-speed asynchronous read directly from the BaseStream
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false);

                    if(bytesRead > 0)
                    {
                        // Copy only what arrived to isolate memory and avoid threading overwrites
                        byte[] dataPayload = new byte[bytesRead];
                        Buffer.BlockCopy(buffer, 0, dataPayload, 0, bytesRead);

                        // Handoff to the channel pipeline instantly—this takes mere nanoseconds
                        _packetChannel.Writer.TryWrite(dataPayload);
                    }
                }
            }
            catch(OperationCanceledException) { /* Normal shutdown scenario */ 
            
            }
            catch(Exception ex)
            {
                Debugger.Log(1,"Exception",$"Critical Stream Error: {ex.Message}");
            }
        }

        // 4. Expose the processed data stream to consumers (UI, Database, Parser) safely
        public IAsyncEnumerable<byte[]> ConsumeDataAsync(CancellationToken token)
        {
            return _packetChannel.Reader.ReadAllAsync(token);
        }

        public async ValueTask DisposeAsync()
        {
            if(_cts != null && !_cts.IsCancellationRequested)
            {

                await _cts.CancelAsync();
            }
            //if(_readTask != null)
            //{
            //    try
            //    {
            //        await _readTask.;
            //    }
            //    catch { /* suppress */ }
            //}

            _serialPort?.Close();
            _serialPort?.Dispose();
            _cts?.Dispose();
        }
    }

}