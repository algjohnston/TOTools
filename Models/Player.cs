namespace TOTools.Models;

/// <summary>
/// Alexander Johnston
/// Represents a player in the tournaments.
/// </summary>
/// <param name="startggId">The startgg id of the player.</param>
/// <param name="tag">The tag (aka gamertag) of the player.</param>
/// <param name="region">The region the player resides.</param>
/// <param name="tier">The tier the player is in</param>
/// <param name="ranking">The rank of player within the given tier.</param>
public class Player(string startggId, string tag, Region region, Tier tier, int ranking) : IComparable<Player>
{
    public string StarttggId { get; } = startggId;

    public string PlayerTag { get; } = tag;

    public Region PlayerRegion { get; } = region;

    public Tier PlayerTier { get; } = tier;

    public int PlayerRanking { get; } = ranking;

    public string FormattedPlayer => $"{StarttggId} : {PlayerTag}: {PlayerRegion} : {PlayerTier} : {PlayerRanking}";

    public string FormattedPlayerForList => $"{PlayerTag}: {RegionHelper.ConvertToString(PlayerRegion)} : {PlayerTier}";

    public int CompareTo(Player? other)
    {
        if (other is null) return 1;
        if (this.PlayerTier - other.PlayerTier == 0)
        {
            return PlayerRanking - other.PlayerRanking;
        }
        else return this.PlayerTier - other.PlayerTier;
    }
}