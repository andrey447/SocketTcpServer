using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace SocketTcpServer
{
    class Program
    {
        static int port = 8888;
        static IPAddress ip = IPAddress.Parse("127.0.0.1");

        static void Main(string[] args)
        {
            IPEndPoint localEndPoint = new IPEndPoint(ip, port);

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(localEndPoint);

                int numberOfPendingConnections = 10;
                listenSocket.Listen(numberOfPendingConnections);

                Console.WriteLine("Сервер запущен. Ожидание подключений...");

                while (true)
                {
                    Socket handler = listenSocket.Accept();

                    // receive the message
                    StringBuilder receivedMessage = new StringBuilder();
                    int receivedBytes = 0;
                    byte[] receivedDataBuffer = new byte[256];

                    do
                    {
                        receivedBytes = handler.Receive(receivedDataBuffer);
                        receivedMessage.Append(Encoding.Unicode.GetString(receivedDataBuffer, 0, receivedBytes));
                    }
                    while (handler.Available > 0);

                    Console.WriteLine(DateTime.Now.ToShortTimeString() + ": " + receivedMessage.ToString());

                    // send the message
                    string message = "ваше сообщение доставлено";
                    byte[] sendDataBuffer = Encoding.Unicode.GetBytes(message);
                    handler.Send(sendDataBuffer);

                    // close socket
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}