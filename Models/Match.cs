using CS341Project.Models;

namespace TOTools.Models;

/// <summary>
/// Alexander Johnston
/// Represents a match between two players with a time estimate
/// on how long the match could take.
/// </summary>
/// <param name="player1">The first player.</param>
/// <param name="player2">The second player.</param>
/// <param name="timeInSeconds">
/// How long the match is estimated to complete, in seconds.
/// This will be used to estimate future match times for the given players.
/// </param>
/// <param name="game">The game that will be played</param>
/// <param name="isBestOfFive"> Whether the match is BO5 or BO3.</param>
public class Match(string player1, string player2, long timeInSeconds, Game game, bool isBestOfFive)
{
    public string Player1 { get; } = player1;

    public string Player2 { get; } = player2;

    public long TimeInSeconds { get; } = timeInSeconds;

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

    public Game GameName { get; } = game;
    
    public bool IsBestOfFive { get; } = isBestOfFive;

    public string FormattedMatch => $"{Player1} vs. {Player2} {GameName}";
}