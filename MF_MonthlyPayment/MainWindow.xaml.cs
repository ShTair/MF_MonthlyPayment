using CsvHelper;
using MF_MonthlyPayment.Models;
using System;
using System.Collections.Generic;
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
            var today = DateTime.Today;
            var nm = today.AddDays(1 - today.Day);

            using (var writer = File.CreateText("test.txt"))
            using (var csv = new CsvWriter(writer))
            {
                csv.Configuration.RegisterClassMap<PaymentItem.Map>();

                csv.WriteHeader<PaymentItem>();
                csv.WriteRecords(CreateItems(nm, ContentBox.Text, int.Parse(AmountBox.Text), int.Parse(CountBox.Text), Kind1Box.Text, Kind2Box.Text));
            }
        }

        private IEnumerable<PaymentItem> CreateItems(DateTime start, string content, int amount, int count, string kind1, string kind2)
        {
            var limit = DateTime.Today.AddYears(1);
            var date = start;

            for (int i = 0; i < count; i++)
            {
                var v = amount / (count - i);

                yield return new PaymentItem
                {
                    Date = date,
                    Content = content,
                    Amount = -v,
                    Bank = "前払いプール",
                    Kind1 = kind1,
                    Kind2 = kind2,
                };

                yield return new PaymentItem
                {
                    Date = date,
                    Content = content,
                    Amount = v,
                    Bank = "前払い元",
                    Kind1 = "収入",
                    Kind2 = "その他入金",
                };

                amount -= v;

                var next = start.AddMonths(i + 1);
                if (next > limit)
                {
                    yield return new PaymentItem
                    {
                        Date = date,
                        Content = $"{content} 残り{count - i - 1}回",
                        Amount = -amount,
                        Bank = "前払いプール",
                        Kind1 = kind1,
                        Kind2 = kind2,
                    };

                    yield return new PaymentItem
                    {
                        Date = date,
                        Content = $"{content} 残り{count - i - 1}回",
                        Amount = amount,
                        Bank = "前払い元",
                        Kind1 = "収入",
                        Kind2 = "その他入金",
                    };

                    break;
                }

                date = next;
            }
        }
    }
}
