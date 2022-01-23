using DraftDesktopApp.Models.Entities;
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DraftDesktopApp.Converters
{
    public class MaterialBackgroundValueConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            Material material = value as Material;
            BrushConverter brushConverter = new BrushConverter();
            if (material.CountInStock < material.MinCount)
            {
                return brushConverter.ConvertFrom("#f19292");
            }
            else if (material.CountInStock >= material.MinCount * 3)
            {
                return brushConverter.ConvertFrom("#ffba01");
            }
            return Brushes.White;
        }

        public object ConvertBack(object value,
                                  Type targetType,
                                  object parameter,
                                  CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
