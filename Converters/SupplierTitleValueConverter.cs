using DraftDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace DraftDesktopApp.Converters
{
    public class SupplierTitleValueConverter : IValueConverter
    {
        public object Convert(object value,
                              Type targetType,
                              object parameter,
                              CultureInfo culture)
        {
            HashSet<Supplier> suppliers = (HashSet<Supplier>)value;
            IEnumerable<string> suppliersTitles = suppliers.Select(s => s.Title);
            if (suppliersTitles.Count() == 0)
            {
                return "Отсутствуют";
            }
            return string.Join(", ", suppliersTitles);
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
