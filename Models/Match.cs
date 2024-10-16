namespace CS341Project.Models;

/// <summary>
/// Alexander Johnston
/// Represents a match between two players with a time estimate
/// on how long the match could take.
/// </summary>
/// <param name="player1">The first player.</param>
/// <param name="player2">The second player.</param>
/// <param name="estimatedSeconds">
/// The estimated time, in seconds,
/// of the match between the two given players.
/// </param>
public class Match(string player1, string player2, long estimatedSeconds)
{
    public Player Player1 { get; }

    public Player Player2 { get; }

    /// <summary>
    /// Provides the estimated time in mm:ss format
    /// where mm are the minutes and ss are the seconds.
    /// mm may be more than two characters
    /// </summary>
    public string EstimatedTime
    {
        get
        {
            var minutes = estimatedSeconds / 60;
            var seconds = estimatedSeconds % 60;
            return $"{minutes}:{seconds:00}";
        }
    }

    public string FormattedPlayers => $"{player1} vs. {player2}";
}