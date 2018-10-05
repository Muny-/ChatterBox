using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using ChatterBox.Packets;

namespace ChatterBox
{
    public sealed partial class Client : IDisposable
    {
        public void SendPacket(IPacket packet)
        {
            SendLine(packet.Serialize());
        }

        public void SendLine(string line)
        {
            Console.WriteLine($"[->] {line}");
            _streamWriter.WriteLine(line);
            _streamWriter.Flush();
        }

        // Consumers register to receive data.
        public event EventHandler<DataReceivedEventArgs> DataReceived;

        public event EventHandler<EventArgs> Disconnected;

        public event EventHandler<EventArgs> Connected;

        public Client(string host, int port)
        {
            _client = new TcpClient(host, port);
            _stream = _client.GetStream();

            _streamReader = new StreamReader(_stream);
            _streamWriter = new StreamWriter(_stream);

            _readThread = new Thread(ReadRun);
            _readThread.Start();
        }

        private void ReadRun()
        {
            while (_client.Connected)
            {
                string line = _streamReader.ReadLine();

                if (DataReceived != null)
                    DataReceived(this, CreateMockDataReceivedEventArgs(line));
            }

            if (Disconnected != null) Disconnected(this, new EventArgs());
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private DataReceivedEventArgs CreateMockDataReceivedEventArgs(string TestData)
        {
            /*if (String.IsNullOrEmpty(TestData))
                throw new ArgumentException("Data is null or empty.", "Data");*/

            DataReceivedEventArgs MockEventArgs =
                (DataReceivedEventArgs)System.Runtime.Serialization.FormatterServices
                 .GetUninitializedObject(typeof(DataReceivedEventArgs));

            System.Reflection.FieldInfo[] EventFields = typeof(DataReceivedEventArgs)
                .GetFields(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.DeclaredOnly);

            if (EventFields.Count() > 0)
            {
                EventFields[0].SetValue(MockEventArgs, TestData);
            }
            else
            {
                throw new ApplicationException("Failed to find _data field!");
            }

            return MockEventArgs;
        }

        private TcpClient _client;
        private NetworkStream _stream;
        private StreamReader _streamReader;
        private StreamWriter _streamWriter;
        private Thread _readThread;
    }
}
