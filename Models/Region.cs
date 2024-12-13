namespace TOTools.Models;

/// <summary>
/// A list of possible regions a player could be from.
/// </summary>
public enum Region
{
    Milwaukee,
    Madison,
    Norcen,
    West,
    Whitewater,
    OutOfState,
    Unknown
}

public static class RegionHelper
{

    public static Color GetRegionColor(this Region region)
    {
        switch (region)
        {
            case Region.Milwaukee:
                return Colors.Red;
                break;
            case Region.Madison:
                return Colors.Orange;
                break;
            case Region.Norcen:
                return Colors.Yellow;
                break;
            case Region.West:
                return Colors.Green;
                break;
            case Region.Whitewater:
                return Colors.Blue;
                break;
            case Region.OutOfState:
                return Colors.Purple;
                break;
            case Region.Unknown:
                return Colors.Grey;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(region), region, null);
        }
    }
    public static List<string> RegionStrings { get; } = GetRegionStrings();

    /// <summary>
    /// Converts the Region enum values to a list of strings.
    /// </summary>
    /// <returns>A list of strings representing each region.</returns>
    private static List<string> GetRegionStrings()
    {
        return Enum.GetValues<Region>()
            .Select(ConvertToString)
            .ToList();
    }
    
    /// <summary>
    /// Converts an integer to a region enum.
    /// </summary>
    /// <param name="regionInt">The index of the region enum.</param>
    /// <returns>The region enum represented by the given integer.</returns>
    public static Region ConvertToRegion(int regionInt)
    {
        if (Enum.IsDefined(typeof(Region), regionInt))
        {
            return (Region)regionInt;
        }

        return Region.Unknown;
    }

    /// <summary>
    /// Converts a region to a string.
    /// </summary>
    /// <param name="region">The region to convert to a string.</param>
    /// <returns>A string representation of the given region.</returns>
    public static string ConvertToString(Region region)
    {
        return region switch
        {
            Region.Milwaukee => "Milwaukee",
            Region.Madison => "Madison",
            Region.Norcen => "Norcen",
            Region.West => "West",
            Region.Whitewater => "Whitewater",
            Region.OutOfState => "Out of State",
            _ => "Unknown Region"
        };
    }
}