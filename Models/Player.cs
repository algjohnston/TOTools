namespace CS341Project.Models;

/// <summary>
/// Represents a player in the tournaments.
/// </summary>
/// <param name="name">The name of the player.</param>
/// <param name="region">The region the player resides.</param>
public class Player(string name, string region)
{
    public string Name { get; } = name;
    public string Region { get; } = region;
    
    public string FormattedPlayer => $"{Name}: {Region}";
}