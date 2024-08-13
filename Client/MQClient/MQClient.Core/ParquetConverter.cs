using Parquet;
using Parquet.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQClient.Core
{
    public class ParquetConverter
    {
        public static DataTable ConvertParquetToDataTable(string parquetFilePath)
        {
            DataTable dataTable = new DataTable();

            using (Stream fileStream = System.IO.File.OpenRead(parquetFilePath))
            {
                // ParquetReaderでファイルを読み込む
                using (var parquetReader = new ParquetReader(fileStream))
                {
                    // Parquetファイルに含まれる全てのデータフィールドを取得
                    DataField[] dataFields = parquetReader.Schema.GetDataFields();

                    // DataTable にカラムを追加
                    foreach (var field in parquetReader.Schema.Fields)
                    {
                        dataTable.Columns.Add(field.Name, typeof(object));
                    }

                    Parquet.Data.DataColumn[] columns = parquetReader.ReadEntireRowGroup();


                    // DataTable に行を追加
                    int rowCount = columns[0].Data.Length;
                    for (int i = 0; i < rowCount; i++)
                    {
                        DataRow row = dataTable.NewRow();
                        for (int j = 0; j < columns.Length; j++)
                        {
                            row[j] = columns[j].Data.GetValue(i);
                        }
                        dataTable.Rows.Add(row);
                    }

                }
            }

            return dataTable;
        }


        //private static void ReadParquetWithTable(string parquetFilePath)
        //{
        //    using (Stream fileStream = System.IO.File.OpenRead(parquetFilePath))
        //    {
        //        // ParquetReaderでファイルを読み込む
        //        using (var parquetReader = new ParquetReader(fileStream))
        //        {
        //            var table = parquetReader.ReadAsTable();

        //            foreach (var row in table)
        //            {
        //                foreach (var cell in row)
        //                {
        //                    Console.WriteLine(cell);
        //                }
        //            }
        //        }
        //    }
        //}


        //private static void ReadParquet(string parquetFilePath)
        //{
        //    using (Stream fileStream = System.IO.File.OpenRead(parquetFilePath))
        //    {
        //        // ParquetReaderでファイルを読み込む
        //        using (var parquetReader = new ParquetReader(fileStream))
        //        {
        //            // Parquetファイルに含まれる全てのデータフィールドを取得
        //            DataField[] dataFields = parquetReader.Schema.GetDataFields();

        //            // 各RowGroupをループして読み取る
        //            for (int i = 0; i < parquetReader.RowGroupCount; i++)
        //            {
        //                // RowGroupを読み込む
        //                using (ParquetRowGroupReader groupReader = parquetReader.OpenRowGroupReader(i))
        //                {
        //                    // 各カラムのデータを読み取る
        //                    foreach (DataField field in dataFields)
        //                    {
        //                        Array data = groupReader.ReadColumn(field).Data;
        //                        Console.WriteLine($"Column: {field.Name}");
        //                        foreach (var value in data)
        //                        {
        //                            Console.WriteLine(value);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

    }
}
