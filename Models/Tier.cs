namespace TOTools.Models;

/// <summary>
/// A list of possible tiers a player could be in.
/// </summary>
public enum Tier
{
    S,
    A,
    B,
    C,
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

public class TierConverter
{
    public string ToString(Tier tier)
    {
        return tier switch
        {
            Tier.S => "S",
            Tier.A => "A",
            Tier.B => "B",
            Tier.C => "C",
            Tier.D => "D",
            Tier.E => "E",
            Tier.F => "F",
        };
    }
}