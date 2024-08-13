using NetMQ;
using NetMQ.Sockets;
using Parquet;
using Parquet.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var requestSocket = new RequestSocket())
            {
                // サーバーに接続
                requestSocket.Connect("tcp://localhost:5555");

                // サーバーにメッセージを送信
                Console.WriteLine("Sending Hello");
                requestSocket.SendFrame("df[\"NewCol\"] = df[\"Age\"] * 10");

                // サーバーからの返信を受信
                string parquetFilePath = requestSocket.ReceiveFrameString();
                Console.WriteLine("Received reply: {0}", parquetFilePath);

                //ReadParquet(parquetFilePath);
                ReadParquetWithTable(parquetFilePath);
            }
        }

        private static void ReadParquet(string parquetFilePath)
        {
            using (Stream fileStream = System.IO.File.OpenRead(parquetFilePath))
            {
                // ParquetReaderでファイルを読み込む
                using (var parquetReader = new ParquetReader(fileStream))
                {
                    // Parquetファイルに含まれる全てのデータフィールドを取得
                    DataField[] dataFields = parquetReader.Schema.GetDataFields();

                    // 各RowGroupをループして読み取る
                    for (int i = 0; i < parquetReader.RowGroupCount; i++)
                    {
                        // RowGroupを読み込む
                        using (ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(i))
                        {
                            // 各カラムのデータを読み取る
                            foreach (DataField field in dataFields)
                            {
                                Array data = groupReader.ReadColumn(field).Data;
                                Console.WriteLine($"Column: {field.Name}");
                                foreach (var value in data)
                                {
                                    Console.WriteLine(value);
                                }
                            }
                        }
                    }
                }
            }
        }



        private static void ReadParquetWithTable(string parquetFilePath)
        {
            using (Stream fileStream = System.IO.File.OpenRead(parquetFilePath))
            {
                // ParquetReaderでファイルを読み込む
                using (var parquetReader = new ParquetReader(fileStream))
                {
                    var table = parquetReader.ReadAsTable();

                    foreach (var row in table)
                    {
                        foreach (var cell in row)
                        {
                            Console.WriteLine(cell);
                        }
                    }
                }
            }
        }
    }
}
