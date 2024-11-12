﻿using CS341Project.Models;

namespace TOTools.Models;

/// <summary>
/// Alexander Johnston
/// Represents a match between two players with a time estimate
/// on how long the match could take.
/// </summary>
/// <param name="id">The autogenerated id of the match.</param>
/// <param name="player1">The first player.</param>
/// <param name="player2">The second player.</param>
/// <param name="timeInSeconds">
/// <param name="game"> the game they are plaing.</param>
/// /// <param name="isBestOfFive"> whether the match was BO5 or BO3.</param>
/// How long the match took to complete, in seconds.
/// This will be used to estimate future match times for the given players.
/// /// </param>
public class Match(long id, string player1, string player2, long? timeInSeconds, Game game, bool isBestOfFive)
{
    public long MatchId { get; } = id;
    
    public string Player1 { get; } = player1;

    public string Player2 { get; } = player2;
    
    public long TimeInSeconds { get; } = timeInSeconds ?? 0;
    
    public bool isBestOfFive { get; } = isBestOfFive;

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

    public string FormattedMatch => $"{Player1} vs. {Player2} {GameName}";
}