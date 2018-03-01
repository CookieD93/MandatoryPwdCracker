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

        private TcpClient connectionSocket;
        private TcpListener serverSocket;
        String uri = @"D:/Testfolder";
        FileStream fileStream;

        public EchoService(TcpClient connectionSocket)
        {
            // TODO: Complete member initialization
            this.connectionSocket = connectionSocket;
        }

        public EchoService(ref TcpClient connectionSocket, ref TcpListener serverSocket)
        {
            // TODO: Complete member initialization
            this.connectionSocket = connectionSocket;
            this.serverSocket = serverSocket;
        }
        internal void DoIt()
        {
            ns = connectionSocket.GetStream();
            sr = new StreamReader(ns);
            // StreamWriter sw = new StreamWriter(ns);
            sw = new StreamWriter(new BufferedStream(ns));
            sw.AutoFlush = true; // enable automatic flushing

            string message = sr.ReadLine();
            string answer, first;
            while (message != null && message != "")
            {
                Console.WriteLine("Client: " + message);
                string[] list = message.Split(' '); //GET index.htm http/1.1
                first = list[0].ToUpper();
                if (first.Equals("STOP"))
                {
                    Console.WriteLine("Wants to stop");
                    ns.Close();
                    connectionSocket.Close();
                    while (serverSocket.Pending())
                        Thread.Sleep(100);
                    Console.WriteLine("Shutdown says; Server is stopped");

                    serverSocket.Stop();
                    break;
                }

                if (first.Equals("GET") && list.Length == 3)
                {
                    string fileName = list[1];
                    string protocol = list[2];

                    //Ass. 2

                    //sw.WriteLine("Requested file: " + fileName);

                    //Ass. 3
                    sw.Write("HTTP/1.1 200 OK\r\n");
                    sw.Write("Content-Type: image/jpg\r\n");
                    sw.Write("Connection: close\r\n");
                    sw.Write("\r\n"); //To Browser, marks end of header and data are coming
                 // sw.Write("Hello client\r\n"); //Not to browser
                  // sw.Write("Requested file: " + fileName + "\r\n"); //Not to browser
                    
                    //Ass. 5                 
                    uri = uri + fileName;
                    // Read as bytes calling this method:
                    //ReadAndDisplayFilesAsync(uri, sw);
                    //OR JUST

                    fileStream = new FileStream(uri, FileMode.Open, FileAccess.Read);
                    fileStream.CopyTo(sw.BaseStream);
                  
                    // OR do as below:

                   
                    //StreamReader fileReader = new StreamReader(fileStream);
                    
                    //while (!fileReader.EndOfStream)
                    //{
                    //    string s = fileReader.ReadLine();
                    //    Console.WriteLine(s);
                    //    sw.Write(s + "\r\n");
                    //}
                    //sw.Write("\r\n");
                    sw.Flush();
                    sw.BaseStream.Flush();
                    sw.Close();

                }
                else
                {
                    answer = message.ToUpper();
                    sw.WriteLine(answer);
                }
                // sw.Flush();
                message = sr.ReadLine();

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

