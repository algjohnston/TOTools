namespace CS341Project.Models;

/// <summary>
/// Alexander Johnston
/// Represents a match between two players with a time estimate
/// on how long the match could take.
/// </summary>
/// <param name="player1">The first player.</param>
/// <param name="player2">The second player.</param>
/// <param name="timeInSeconds">how long the match took to complete
/// The estimated time, in seconds,
/// of the match between the two given players.
/// </param>
public class Match(long id, string player1, string player2, long timeInSeconds)
{
    public long MatchId { get; } = id;
    
    public string Player1 { get; } = player1;

    public string Player2 { get; } = player2;

    /// <summary>
    /// Provides the match time in mm:ss format
    /// where mm are the minutes and ss are the seconds.
    /// mm may be more than two characters
    /// </summary>
    public string MatchTime
    {
        get
        {
            var minutes = timeInSeconds / 60;
            var seconds = timeInSeconds % 60;
            return $"{minutes}:{seconds:00}";
        }
    }

    public string FormattedPlayers => $"{Player1} vs. {Player2}";
}