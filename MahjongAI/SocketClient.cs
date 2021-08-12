using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Diagnostics;

namespace MahjongAI
{
    class SocketClient
    {
        const int bufferSize = 1024;

        private TcpClient client;
        private NetworkStream stream;

        public SocketClient(string server, int port)
        {
            int retries = 5;
            while (retries-- > 0)
            {
                try
                {
                    client = new TcpClient(server, port);
                    client.ReceiveTimeout = 20000;
                    stream = client.GetStream();
                    return;
                }
                catch { }
            }

            throw new SocketException();
        }

        ~SocketClient()
        {
            Close();
        }

        public void Close()
        {
            stream.Close();
            client.Close();
        }

        public void Send(string message)
        {
            try {
                byte[] data = Encoding.ASCII.GetBytes(message + "\0");
                stream.Write(data, 0, data.Length);
                Trace.TraceInformation("Send: {0}", message);
            }
            catch (Exception)
            {
                throw new SocketException();
            }
        }

        public string Receive()
        {
            byte[] data = new byte[bufferSize];
            int bytes = stream.Read(data, 0, data.Length);
            string responseData = Encoding.ASCII.GetString(data, 0, bytes);
            if (!string.IsNullOrEmpty(responseData))
            {
                Trace.TraceInformation("Receive: {0}", responseData);
            }
            return responseData;
        }
    }
}
