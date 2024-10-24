namespace CS341Project.Models;

/// <summary>
/// Alexander Johnston
/// Represents a player in the tournaments.
/// </summary>
/// <param name="tag">The tag (aka gamertag) of the player.</param>
/// <param name="region">The region the player resides.</param>
/// <param name="tier">The tier the player is in
/// <param name="ranking> ranking of player within tier
public class Player(long id, string tag, string region, Tier tier, int ranking)
{

    public long PlayerId { get; } = id;
    
    public string PlayerTag { get; } = tag;

    public string PlayerRegion { get; } = region;

    public Tier PlayerTier { get; } = tier;

    public int PlayerRanking { get; } = ranking;
    
    public string FormattedPlayer => $"{PlayerId} : {PlayerTag}: {PlayerRegion} : {PlayerTier} : {PlayerRanking}";
    
}