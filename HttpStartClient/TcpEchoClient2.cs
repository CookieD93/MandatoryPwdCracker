/*
 * TCPEchoClient
 *
 * Author Michael Claudius, ZIBAT Computer Science
 * Version 1.0. 2014.02.10
 * Copyright 2014 by Michael Claudius
 * Revised 2014.09.01, 2016.09.14
 * All rights reserved
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

namespace HttpStartClient
{
    class TCPEchoClient
    {
        public static List<string> list;
        static void Main(string[] args)
        {
            list = GetDictionary();
            Cracking crack = new Cracking();
            //Console.ReadLine();
            TcpClient clientSocket = new TcpClient("127.0.0.1", 6789);
            Console.WriteLine("Client ready");

            Stream ns = clientSocket.GetStream(); //provides a Stream
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing

            string message = Console.ReadLine();
            sw.WriteLine(message);
            string serverAnswer = sr.ReadLine();
            string trigger = "on";
            while (trigger != $"off")
            {
                message = Console.ReadLine();
                sw.WriteLine(message);
                Console.WriteLine("Server: " + serverAnswer);
                serverAnswer = sr.ReadLine();
                if (message =="off")
                {
                    trigger = "off";
                }
            }
            Console.WriteLine("No more from server. Press Enter");
            Console.ReadLine();

            ns.Close();

            clientSocket.Close();

        }

        public static List<string> GetDictionary()
        {
            List<string> dictionaryList = new List<string>();
            using (FileStream fs = new FileStream("webster-dictionary.txt", FileMode.Open, FileAccess.Read))
            {
                using (StreamReader dictionary = new StreamReader(fs))
                {
                    while (!dictionary.EndOfStream)
                    {
                        dictionaryList.Add(dictionary.ReadLine());
                    }
                }
                return dictionaryList;
            }
        }
    }

    }

