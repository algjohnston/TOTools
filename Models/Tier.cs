namespace CS341Project.Models;

/// <summary>
/// A list of possible tiers a player could be in.
/// </summary>
public enum Tier
{
    S,
    A,
    B,
    D,
    E,
    F
}

public static class TierHelper
{
    public static Tier ConvertToTier(int tierInt)
    {
        if (Enum.IsDefined(typeof(Tier), tierInt))
        {
            return (Tier)tierInt;
        }

        return Tier.F; 
    }
}