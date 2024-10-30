using System.Globalization;
namespace CS341Project.EventMap;

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
                Models.Region.Norcen => "North Central",
                Models.Region.West => "Western",
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
