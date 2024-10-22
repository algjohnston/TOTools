namespace CS341Project.Models;

/// <summary>
/// Alexander Johnston
/// Represents a player in the tournaments.
/// </summary>
/// <param name="name">The name of the player.</param>
/// <param name="region">The region the player resides.</param>
public class Player(string name, string region, Tier tier)
{
    public string Name { get; } = name;
    public string Region { get; } = region;
    
    public Tier Tier { get; } = tier;
    
    public string FormattedPlayer => $"{Name}: {Region}";
}