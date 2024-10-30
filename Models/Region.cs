﻿namespace CS341Project.Models;

/// <summary>
/// A list of possible regions a player could be from.
/// </summary>
public enum Region
{
    MILWAUKEE,
    MADISON,
    NORCEN,
    WEST,
    WHITEWATER,
    OUT_OF_STATE,
    UNKNOWN
}

public static class RegionHelper
{
    public static Region ConvertToRegion(int regionInt)
    {
        if (Enum.IsDefined(typeof(Tier), regionInt))
        {
            return (Region)regionInt;
        }

        return Region.UNKNOWN;
    }
}