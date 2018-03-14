
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace HttpStartServer
{
    class EchoService
    {
        Stream ns;
        StreamReader sr;
        StreamWriter sw;
        private int clientNumber;

        private TcpClient connectionSocket;
        private TcpListener serverSocket;
        private static double totalTime = 0;
        private static Stopwatch stopUr;
        String uri = @"D:/Testfolder";
        FileStream fileStream;

        public EchoService(TcpClient connectionSocket)
        {
            this.connectionSocket = connectionSocket;
            TCPEchoServer2.latestClient++;
            clientNumber = TCPEchoServer2.latestClient;
            if (stopUr == null)
            {
                stopUr = new Stopwatch();
            }
        }
        
        public EchoService(ref TcpClient connectionSocket, ref TcpListener serverSocket)
        {
            this.connectionSocket = connectionSocket;
            this.serverSocket = serverSocket;
            TCPEchoServer2.latestClient++;
            clientNumber = TCPEchoServer2.latestClient;
            if (stopUr==null)
            {
                stopUr = new Stopwatch();
            }
        }
        internal void DoIt()
        {
            ns = connectionSocket.GetStream();
            sr = new StreamReader(ns);
            sw = new StreamWriter(new BufferedStream(ns));
            sw.AutoFlush = true; // enable automatic flushing
            string answer, first;
            string message = "First";
            Console.WriteLine("Client{0} " + message, clientNumber);
            message = sr.ReadLine();
            if (!stopUr.IsRunning)
            {
                stopUr.Start();
            }
            while (!string.IsNullOrEmpty(message))
            {
                string[] splitMessage = message.Split('@');

                if (splitMessage.Last().ToLower() == "off" || !TCPEchoServer2.keepRunning)
                {
                    answer = "off";
                    sw.WriteLine(answer);
                    TCPEchoServer2.keepRunning = false;
                }


                if (splitMessage.Last().ToLower() == "ready")
                {
                    int indexToStartAt = TCPEchoServer2.dictionaryIndexCounter * 5000;
                    answer = "crack." + indexToStartAt;
                    sw.WriteLine(answer);
                    TCPEchoServer2.dictionaryIndexCounter++;
                }

                var count = 0;
                foreach (var s in splitMessage)
                {
                    count++;
                    if (s == "time:")
                    {
                        totalTime = totalTime + (TimeSpan.Parse(splitMessage[count]).TotalMilliseconds)/1000;
                        Console.WriteLine("Arbejdstid fra klienternes side: "+totalTime);
                        break;
                    }
                    
                }
                count = 0;
                foreach (var s in splitMessage)
                {
                    count++;
                    if (s == "Index Number: ")
                    {
                        Console.WriteLine("kørt fra index "+splitMessage[count]+" til "+(int.Parse(splitMessage[count])+4999));
                        break;
                    }

                }
                foreach (var s in splitMessage)
                {
                    if (s == "time:" || s == "" || s == "ready")
                    {
                        break;
                    }
                    TCPEchoServer2.pwList.Add(s);
                }
                Console.WriteLine("printer pw liste:");
                foreach (var o in TCPEchoServer2.pwList)
                {
                    Console.WriteLine("+++++" + o);

                }
                Console.WriteLine("TotalTime elapsed: "+stopUr.Elapsed);
                Console.WriteLine(" ");
                message = sr.ReadLine();
            }
        }

        private async static void ReadAndDisplayFilesAsync(String fileName, StreamWriter sw)
        {
            Char[] buffer;
            using (var sr = new StreamReader(fileName))
            {
                buffer = new Char[(int)sr.BaseStream.Length];
                await sr.ReadAsync(buffer, 0, (int)sr.BaseStream.Length);
            }

            String fileText = (new String(buffer));
            Console.WriteLine(fileText);
            sw.WriteLine(fileText);
            sw.Flush();
            sw.BaseStream.Flush();
            sw.Close();


        }

    }
}

