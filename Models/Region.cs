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