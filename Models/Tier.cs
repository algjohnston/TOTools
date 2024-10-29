namespace CS341Project.Models;

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
    public static Tier ConvertToTier(int tierInt) //chatgpt gave me this, I have no idea if it sucks or not
    {
        // Try parsing the string as a Tier enum
        if (Enum.IsDefined(typeof(Tier), tierInt))
        {
            return (Tier)tierInt;
            // Proceed with enumValue
        }

        return Tier.F; // If no match is found, just return F by default
    }
}