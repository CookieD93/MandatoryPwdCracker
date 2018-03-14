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
            TcpClient clientSocket = new TcpClient("127.0.0.1", 6789);

            Stream ns = clientSocket.GetStream(); //provides a Stream
            StreamReader sr = new StreamReader(ns);
            StreamWriter sw = new StreamWriter(ns);
            sw.AutoFlush = true; // enable automatic flushing
            
            Console.WriteLine("Client ready");
            sw.WriteLine("ready");
            string serverAnswer = sr.ReadLine();
            while (serverAnswer != "off")
            {
                string[] splitAnswer = serverAnswer.Split('.');
                Console.WriteLine("Server: " + serverAnswer);
                if (serverAnswer.ToLower().Contains("crack"))
                {
                    string result = crack.RunCracking(Int32.Parse(splitAnswer[1]), sw);
                    sw.WriteLine(result);
                }
                serverAnswer = sr.ReadLine();
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

