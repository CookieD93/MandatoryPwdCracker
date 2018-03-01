/*
 * EchoService.cs
 *
 * Author Michael Claudius, ZIBAT Computer Science
 * Version 1.0. 2014.02.10
 * Copyright 2014 by Michael Claudius
 * Revised 2014.09.11, 2016.03.01, 2017.09.30
 * All rights reserved
 */

using System;
using System.Collections.Generic;
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
        String uri = @"D:/Testfolder";
        FileStream fileStream;

        public EchoService(TcpClient connectionSocket)
        {
            // TODO: Complete member initialization
            this.connectionSocket = connectionSocket;
            TCPEchoServer2.latestClient++;
            clientNumber = TCPEchoServer2.latestClient;
        }

        public EchoService(ref TcpClient connectionSocket, ref TcpListener serverSocket)
        {
            // TODO: Complete member initialization
            this.connectionSocket = connectionSocket;
            this.serverSocket = serverSocket;
            TCPEchoServer2.latestClient++;
            clientNumber = TCPEchoServer2.latestClient;
        }
        internal void DoIt()
        {
            ns = connectionSocket.GetStream();
            sr = new StreamReader(ns);
            // StreamWriter sw = new StreamWriter(ns);
            sw = new StreamWriter(new BufferedStream(ns));
            sw.AutoFlush = true; // enable automatic flushing
            string answer, first;
            string message = "First";
            Console.WriteLine("Client{0} " + message,clientNumber);
            while (!string.IsNullOrEmpty(message))
            {
                answer = Console.ReadLine();
                sw.WriteLine(answer);
                #region udkommenteret
                //Console.WriteLine("Client{0}: " + message,clientNumber);
                //string[] list = message.Split(' '); //GET index.htm http/1.1
                //first = list[0].ToUpper();
                //if (first.Equals("STOP"))
                //{
                //    Console.WriteLine("Client{0} Wants to stop",clientNumber);
                //    ns.Close();
                //    connectionSocket.Close();
                //    while (serverSocket.Pending())
                //        Thread.Sleep(100);
                //    Console.WriteLine("Connection with Client{0} terminated",clientNumber);

                //    serverSocket.Stop();
                //    break;
                //}

                //if (first.Equals("GET") && list.Length == 3)
                //{
                //    string fileName = list[1];
                //    string protocol = list[2];

                //    //Ass. 2

                //    //sw.WriteLine("Requested file: " + fileName);

                //    //Ass. 3
                //    sw.Write("HTTP/1.1 200 OK\r\n");
                //    sw.Write("Content-Type: image/jpg\r\n");
                //    sw.Write("Connection: close\r\n");
                //    sw.Write("\r\n"); //To Browser, marks end of header and data are coming
                // // sw.Write("Hello client\r\n"); //Not to browser
                //  // sw.Write("Requested file: " + fileName + "\r\n"); //Not to browser

                //    //Ass. 5                 
                //    uri = uri + fileName;
                //    // Read as bytes calling this method:
                //    //ReadAndDisplayFilesAsync(uri, sw);
                //    //OR JUST

                //    fileStream = new FileStream(uri, FileMode.Open, FileAccess.Read);
                //    fileStream.CopyTo(sw.BaseStream);

                //    // OR do as below:


                //    //StreamReader fileReader = new StreamReader(fileStream);

                //    //while (!fileReader.EndOfStream)
                //    //{
                //    //    string s = fileReader.ReadLine();
                //    //    Console.WriteLine(s);
                //    //    sw.Write(s + "\r\n");
                //    //}
                //    //sw.Write("\r\n");
                //    sw.Flush();
                //    sw.BaseStream.Flush();
                //    sw.Close();

                //}
                //else
                //{
                //    answer = Console.ReadLine();
                //    sw.WriteLine(answer);
                //} 
                #endregion
                message = sr.ReadLine();
                Console.WriteLine(message);

            }
            //ns.Close();
            //connectionSocket.Close();
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

