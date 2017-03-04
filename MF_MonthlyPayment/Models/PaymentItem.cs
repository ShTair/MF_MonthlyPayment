using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System;

namespace MF_MonthlyPayment.Models
{
    class PaymentItem
    {
        public bool IsEnable => true;

        public DateTime Date { get; set; }

        public string Content { get; set; }

        public int Amount { get; set; }

        public string Bank { get; set; }

        public string Kind1 { get; set; }

        public string Kind2 { get; set; }

        public class Map : CsvClassMap<PaymentItem>
        {
            public Map()
            {
                Map(m => m.IsEnable).Name("計算対象").TypeConverter<IsEnableTypeConverter>();
                Map(m => m.Date).Name("日付").TypeConverter<DateTypeConverter>();
                Map(m => m.Content).Name("内容");
                Map(m => m.Amount).Name("金額");
                Map(m => m.Bank).Name("金融機関");
                Map(m => m.Kind1).Name("大項目");
                Map(m => m.Kind2).Name("中項目");
            }

            private abstract class TypeConverter<T> : ITypeConverter
            {
                public bool CanConvertFrom(Type type) => true;

                public bool CanConvertTo(Type type) => true;

                public object ConvertFromString(TypeConverterOptions options, string text) => ConvertTypeFromString(options, text);

                public string ConvertToString(TypeConverterOptions options, object value) => ConvertTypeToString(options, (T)value);

                public abstract T ConvertTypeFromString(TypeConverterOptions options, string text);

                public abstract string ConvertTypeToString(TypeConverterOptions options, T value);
            }

            private class IsEnableTypeConverter : TypeConverter<bool>
            {
                public override bool ConvertTypeFromString(TypeConverterOptions options, string text)
                {
                    return text == "1";
                }

                public override string ConvertTypeToString(TypeConverterOptions options, bool value)
                {
                    return value ? "1" : "0";
                }
            }

            private class DateTypeConverter : TypeConverter<DateTime>
            {
                public override DateTime ConvertTypeFromString(TypeConverterOptions options, string text)
                {
                    return DateTime.Parse(text);
                }

                public override string ConvertTypeToString(TypeConverterOptions options, DateTime value)
                {
                    return value.ToString("yyyy/M/d");
                }
            }
        }
    }
}
