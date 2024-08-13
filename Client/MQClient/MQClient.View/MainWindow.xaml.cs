using MQClient.Core;
using System;
using System.Collections.Generic;
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

            var parquetFilePath = ZeroMQInteraction.SyncRequest(5555, pythonCode);

            var dataTable = ParquetConverter.ConvertParquetToDataTable(parquetFilePath);
            dataGrid.ItemsSource = dataTable.DefaultView;
        }
    }
}
