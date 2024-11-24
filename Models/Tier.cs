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
    /// <summary>
    /// Converts an integer to a tier enum.
    /// </summary>
    /// <param name="tierInt">The index of the tier enum.</param>
    /// <returns>The tier enum represented by the given integer.</returns>
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
    /// <summary>
    /// Converts a tier to a string.
    /// </summary>
    /// <param name="tier">The tier to convert to a string.</param>
    /// <returns>A string representation of the given tier.</returns>
    public static string ToString(Tier tier)
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
            _ => throw new ArgumentOutOfRangeException(nameof(tier), tier, null)
        };
    }
}