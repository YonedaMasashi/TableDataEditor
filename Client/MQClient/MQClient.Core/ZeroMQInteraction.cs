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
using System.Xml.Linq;
using MQClient.Core.Msg;
using MQClient.Core.Msg.Request;
using MQClient.Core.Msg.Response;

namespace MQClient.Core
{
    public class ZeroMQInteraction
    {
        public static ResponseMessage SyncRequest(int portNum, RequestMessage msg)
        {
            using (var requestSocket = new RequestSocket())
            {
                // サーバーに接続
                requestSocket.Connect("tcp://localhost:" + portNum);

                // サーバーにメッセージを送信
                Console.WriteLine("Sending Message:" + msg.CreateRequestMsg().ToString());
                requestSocket.SendFrame(msg.CreateRequestMsg().ToString());

                // サーバーからの返信を受信
                return new ResponseMessage(requestSocket.ReceiveFrameString());
            }
        }

    }
}
