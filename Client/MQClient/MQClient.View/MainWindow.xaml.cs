using MQClient.Core;
using MQClient.Core.Msg;
using MQClient.Core.Msg.Request;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MQClient.View
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ExecuteButton_Click(object sender, RoutedEventArgs e)
        {
            string pythonCode = PythonScriptTextBox.Text;

            var requestMessage = new CalculateRequestMessage(
                "Sample",
                "C:\\Work\\practice\\Python\\TableDataEditor",
                new List<string>() { pythonCode });

            var responseMessage = ZeroMQInteraction.SyncRequest(5555, requestMessage);

            if (responseMessage != null)
            {
                if (responseMessage.Status == "Success")
                {
                    var dataTable = ParquetConverter.ConvertParquetToDataTable(responseMessage.OutputFilePath);
                    dataGrid.ItemsSource = dataTable.DefaultView;

                    dataGrid.Visibility = Visibility.Visible;
                    TbErrorMessage.Visibility = Visibility.Collapsed;
                } else
                {
                    TbErrorMessage.Text = responseMessage.ExceptionMessage;
                    dataGrid.Visibility = Visibility.Collapsed;
                    TbErrorMessage.Visibility = Visibility.Visible;
                }
            }
        }
    }
}
