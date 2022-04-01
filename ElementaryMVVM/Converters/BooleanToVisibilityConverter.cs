using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ElementaryMVVM.Converters
{
    /// <summary>
    /// Конвертирует bool в Visibility.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Прямое конвертирование bool в Visibility.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Не используется.</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Не используется.</param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null && !(value is bool)) return Visibility.Collapsed;
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Обратное конвертирование Visibility в bool.
        /// </summary>
        /// <param name="value">Конвертируемое значение.</param>
        /// <param name="targetType">Не используется.</param>
        /// <param name="parameter">Не используется.</param>
        /// <param name="culture">Не используется.</param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value == null && !(value is Visibility)) return false;
            if ((Visibility)value == Visibility.Visible) return true;
            else return false;
        }
    }
}
