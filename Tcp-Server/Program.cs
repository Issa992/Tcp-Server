using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Tcp_Server
{
    class Program
    {
        private static readonly int PortNum = 7;

        static void Main(string[] args)
        {
            IPAddress localAddress = IPAddress.Loopback;
            //listener the server is listning for requests 
            TcpListener serverSocket = new TcpListener(localAddress, PortNum);
            serverSocket.Start();
            Console.WriteLine("Tcp server is running on port" + PortNum);
            while (true)
            {
                try
                {
                    //waiting for incoming client until some body hit the ip and port number
                    var client = serverSocket.AcceptTcpClient();
                    Console.WriteLine("Incoming client");
                    //calling DoIt Method for 2 connection task run give new service all the time 
                    Task.Run(() => DoIt(client));
                }
                catch (IOException e)
                {
                    Console.WriteLine("Exception will continue");
                }
            }
        }

        private static void DoIt(TcpClient client)
        {
            //stream is like a pipe goes from client to the server by writing and reading data.
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream);

            //while loop for more connection
            while (true)
            {
                var request = reader.ReadLine();
                //protocol to stop the connection
                if (request == "stop")
                {
                    Console.WriteLine("Server stoped");
                    break;
                }

                if (string.IsNullOrEmpty(request)) break;
                {
                }
                if (request == "add")
                {
                    Console.WriteLine("Illegal request add numbers ");
                }

                if (request.ToLower().Contains("close"))
                {
                    client.Close();
                }

                Console.WriteLine("Request  " + request);
                ConversionWeight(request);

                writer.Flush();
            }

            client.Close();
        }

        private static void ConversionWeight(string Request)
        { 
                try
                {
                    var splitRequest = Request.Split();
                    switch (splitRequest[0])
                    {
                        case "TOOUNCES":
                            double result =
                                GramToOuncesClassLibrary.Conversion.ConvertToOunce(double.Parse(splitRequest[1]));
                            Console.WriteLine("the equivalent in Ounces is " + result);
                            break;
                        case "TOGRAM":
                            double result1 =
                                GramToOuncesClassLibrary.Conversion.ConvertToGram(double.Parse(splitRequest[1]));
                            Console.WriteLine("the equivalent in gram is " + result1);
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error", e);
                }
            
        }
    }
}