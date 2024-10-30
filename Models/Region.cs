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
    public static Region ConvertToRegion(int regionInt)
    {
        if (Enum.IsDefined(typeof(Region), regionInt))
        {
            return (Region)regionInt;
        }

        return Region.Unknown;
    }
    
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

