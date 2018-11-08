using System;
using System.Globalization;
using System.Windows.Data;
using BusinessLogic.Model;

namespace WpfGUI.Converters
{
    [ValueConversion(typeof(ItemTypeEnum), typeof(string))]
    public class ItemTypeEnumToStringConverter : IValueConverter
    {
#pragma warning disable SA1401 // Fields must be private
        public static ItemTypeEnumToStringConverter Instance = new ItemTypeEnumToStringConverter();
#pragma warning restore SA1401 // Fields must be private

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "["+(ItemTypeEnum)value+"]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
