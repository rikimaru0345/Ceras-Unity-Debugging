using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using Ceras;
using SharpDX;
using Glue;

namespace GlueSender
{
    class Program
    {
        static void Main(string[] args)
        {
            SharpDX.Vector3 vector3;
            // Setup serializer
            SerializerConfig config = new SerializerConfig();
            CerasSerializer serializer = new CerasSerializer(config);
            byte[] bytes = System.IO.File.ReadAllBytes("../../data/sendframe.489.bin");

            // The following Deserialization doesn't work at all :(
            //try
            //{
            //    Frame frame = new Frame();
            //    serializer.Deserialize<Frame>(ref frame, bytes);
            //    foreach (var e in frame.Cargo)
            //    {
            //        var key = e.Key;
            //        var value = e.Value;
            //        var valueType = value.GetType();
            //        if (valueType.IsArray)
            //        {
            //            Array arrayValue = value as Array;
            //            string s = "";
            //            foreach (var v in arrayValue)
            //                s += $"{v.ToString()}";
            //            Console.WriteLine($"{key}: {s}");
            //        }
            //        else
            //        {
            //            Console.WriteLine($"{key}: {value.ToString()}");
            //        }
            //    }
            //}
            //catch (Exception e)
            //{
            //    Console.WriteLine(e);
            //}

            // Setup socket
            int _sendOnPort = 27183;
            string _sendToIPAdress = "127.0.0.1";
            UdpClient _socket = new UdpClient();

            // Send the bytes
            while (true)
            {
                Console.WriteLine("Sending Frame...");
                _socket.Send(bytes, bytes.Length, _sendToIPAdress, _sendOnPort);
                System.Threading.Thread.Sleep(16);
            }
        }
    }
}
