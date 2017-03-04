using CsvHelper;
using MF_MonthlyPayment.Models;
using System;
using System.IO;
using System.Windows;

namespace MF_MonthlyPayment
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var writer = File.CreateText("test.txt"))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<PaymentItem.Map>();

                csv.WriteHeader<PaymentItem>();
                csv.WriteRecord(new PaymentItem { Amount = 1000, Date = DateTime.Now });
            }
        }
    }
}
