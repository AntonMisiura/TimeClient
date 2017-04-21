using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TimeClient
{
    internal class Program
    {
        private static Socket _clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        private static void Main(string[] args)
        {
            Console.Title = "Client";
            LoopConnect();
            SendLoop();
            Console.ReadLine();
        }

        private static void SendLoop()
        {
            while (true)
            {
                Thread.Sleep(500);
                byte[] buffer = Encoding.ASCII.GetBytes("get time");
                _clientSocket.Send(buffer);

                byte[] receiveBuf = new byte[1024];
                int rec = _clientSocket.Receive(receiveBuf);
                byte[] data = new byte[rec];
                Array.Copy(receiveBuf, data, rec);
                Console.WriteLine("Received: " + Encoding.ASCII.GetString(data));
            }
        }


        private static void LoopConnect()
        {
            var attempts = 0;

            while (!_clientSocket.Connected)
            {
                try
                {
                    attempts++;
                    _clientSocket.Connect(IPAddress.Loopback, 5678);
                }
                catch (SocketException)
                {
                    Console.Clear();
                    Console.WriteLine("connection attempts: " + attempts.ToString());
                }
            }

            Console.Clear();
            Console.WriteLine("Connected");
        }
    }
}
