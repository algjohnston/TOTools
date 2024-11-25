using System.Globalization;

namespace TOTools.EventMap;

/// <summary>
/// Used to convert region enums to strings for XAML.
/// </summary>
public class RegionToStringConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Models.Region region)
        {
            return region switch
            {
                Models.Region.Milwaukee => "Milwaukee",
                Models.Region.Madison => "Madison",
                Models.Region.Norcen => "Norcen",
                Models.Region.West => "West",
                Models.Region.Whitewater => "Whitewater",
                Models.Region.OutOfState => "Out of State",
                _ => "Unknown Region",
            };
        }

        return "Unknown Region";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return null;
    }
}