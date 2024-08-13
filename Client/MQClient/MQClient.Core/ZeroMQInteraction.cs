using NetMQ.Sockets;
using Parquet.Data;
using Parquet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetMQ;

namespace MQClient.Core
{
    public class ZeroMQInteraction
    {
        public static string SyncRequest(int portNum, string message)
        {
            using (var requestSocket = new RequestSocket())
            {
                // サーバーに接続
                requestSocket.Connect("tcp://localhost:" + portNum);

                // サーバーにメッセージを送信
                Console.WriteLine("Sending Hello");
                requestSocket.SendFrame(message);

                // サーバーからの返信を受信
                return requestSocket.ReceiveFrameString();
            }
        }
    }
}
