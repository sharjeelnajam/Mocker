using System.Runtime.InteropServices;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace MockerProject;

public static class Utilities
{
    public static bool IsBrowser()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Create("BROWSER"));
    }
}

public class CountToVisibilityConverter : IValueConverter
{
    public static readonly CountToVisibilityConverter Instance = new CountToVisibilityConverter();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is int count)
        {
            return count == 0;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
