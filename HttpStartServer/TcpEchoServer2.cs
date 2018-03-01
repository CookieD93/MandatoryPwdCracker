/*
 * TcpEchoServer2.cs
 *
 * Author Michael Claudius, ZIBAT Computer Science
 * Version 1.0. 2014.02.10
 * Copyright 2014 by Michael Claudius
 * Revised 2014.09.15, 2016.02.22
 * All rights reserved
 */

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
        public static int latestClient;
        public static void Main(string[] args)
        {
            latestClient = 0;
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            TcpListener serverSocket = new TcpListener(ip, 6789);
            

            //TcpListener serverSocket = new TcpListener(6789);
            serverSocket.Start();
            try
            {
                while (true)
                {
                    TcpClient connectionSocket = serverSocket.AcceptTcpClient();
                    //EchoService service = new EchoService(connectionSocket)
                    EchoService service = new EchoService(ref connectionSocket, ref serverSocket);
                    Console.WriteLine("Client number {0} connected",latestClient);

                    //Task.Factory.StartNew(service.DoIt);
                    // or use delegates 
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

            //serverSocket.Stop();
        }

    }
}
