using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfApp1.ViewModels;

public class RowIndexConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values == null || values.Length < 1 || values[0] == null)
        {
            return string.Empty;
        }

        // Значение не используется, но привязано для обновления
        // Первый параметр - это сам элемент данных
        // Второй параметр - это ItemsControl, но он не нужен для индекса
        
        // Используем AttachedProperty для получения индекса элемента
        return string.Empty;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}