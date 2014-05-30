using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace FGtest
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread server = new Thread(startServer);
            Thread client = new Thread(startClient);

            server.Start();
            Thread.Sleep(1000);
            client.Start();
        }

        public static void startServer()
        {
            TcpListener server = new TcpListener(IPAddress.Parse("127.0.0.1"),5000);

            server.Start();
            Console.WriteLine("start client");
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();

                NetworkStream stream = client.GetStream();

                StringBuilder data = new StringBuilder();

                while (!stream.DataAvailable) ;

                Byte[] bytes = new Byte[client.Available];

                stream.Read(bytes, 0, bytes.Length);

                data.Append(Encoding.UTF8.GetString(bytes));

                byte[] msg = Encoding.UTF8.GetBytes("return from server".ToCharArray());
                if (data.ToString().Equals("get"))
                    stream.Write(msg, 0, msg.Length);
                client.Close();
            }
        }

        public static void startClient()
        {
            string ip = "127.0.0.1";
            Console.WriteLine("start client");
            TcpClient client = new TcpClient(ip, 5000);
            Console.WriteLine("client connected");
            NetworkStream stream = client.GetStream();

            byte[] bmsg = Encoding.UTF8.GetBytes("get");

            stream.Write(bmsg, 0, bmsg.Length);

            while (!stream.DataAvailable) ;

            Byte[] bytes = new Byte[client.Available];

            stream.Read(bytes, 0, bytes.Length);

            Console.WriteLine(Encoding.UTF8.GetString(bytes));
            client.Close();
        }
    }
}
