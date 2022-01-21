﻿using DraftDesktopApp.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            if(!(value is ObservableCollection<Supplier>))
            {
                return "a, b, c, ...";
            }
            ObservableCollection<Supplier> suppliers = (ObservableCollection<Supplier>)value;
            IEnumerable<string> suppliersTitles = suppliers.Select(s => s.Title);
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