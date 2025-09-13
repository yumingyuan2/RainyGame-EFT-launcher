/* ImageSourceConverter.cs
 * License: NCSA Open Source License
 * 
 * Copyright: SPT
 * AUTHORS:
 * waffle.lord
 */

#nullable enable

using SPT.Launcher.Controllers;
using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using System;
using System.Globalization;

namespace SPT.Launcher.Converters
{
    public class ImageSourceConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string valueString || string.IsNullOrEmpty(valueString))
            {
                return null;
            }
            
            try
            {
                if (targetType.IsAssignableFrom(typeof(Bitmap)))
                {
                    return new Bitmap(valueString);
                }

                return null;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                System.Diagnostics.Debug.WriteLine($"Failed to load bitmap from '{valueString}': {ex.Message}");
                return null;
            }
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException("ConvertBack is not supported for ImageSourceConverter");
        }
    }
}
