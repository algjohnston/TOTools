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
    public static Tier ConvertToTier(string tierString) //chatgpt gave me this, I have no idea if it sucks or not
    {
        // Try parsing the string as a Tier enum
        if (Enum.TryParse<Tier>(tierString, out Tier result))
        {
            return result; // If successful, return the enum value
        }

        return Tier.F; // If no match is found, just return F by default
    }
}