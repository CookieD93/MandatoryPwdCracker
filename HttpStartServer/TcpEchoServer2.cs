using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace HttpStartServer
{
    public class TCPEchoServer2
    {
        public static int dictionaryIndexCounter;
        public static int latestClient;
        public static List<string> pwList;
        public static bool keepRunning = true;

        public static void Main(string[] args)
        {
            latestClient = 0;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(ip, 6789);
            dictionaryIndexCounter = 0;
            pwList = new List<string>();
            
            serverSocket.Start();
            try
            {
                Console.WriteLine("# ========== SERVER READY ========== #");
                while (true)
                {
                    TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                    EchoService service = new EchoService(ref connectionSocket, ref serverSocket);
                    Console.WriteLine("Client number {0} connected",latestClient);
                    Task.Factory.StartNew(() => service.DoIt());


                }
            }
            catch (SocketException ex) //because breaking connection by stop command
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                
                if (serverSocket != null) serverSocket.Stop();
            }
        }

    }
}
