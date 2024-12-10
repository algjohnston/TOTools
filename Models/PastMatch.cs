﻿namespace TOTools.Models;

/// <summary>
/// Alexander Johnston
/// Represents a match between two players with how long the match took.
/// </summary>
/// <param name="id">The autogenerated id of the match.</param>
/// <param name="player1">The first player.</param>
/// <param name="player2">The second player.</param>
/// <param name="timeInSeconds">
/// How long the match took to complete, in seconds.
/// This will be used to estimate future match times for the given players.
/// </param>
/// <param name="game">The game that was played</param>
/// <param name="isBestOfFive"> Whether the match was BO5 or BO3.</param>
public class PastMatch(long id, string player1, string player2, long timeInSeconds, Game game, bool isBestOfFive)
    : Match(null, player1, player2, timeInSeconds, game, isBestOfFive)
{
    public long MatchId { get; } = id;
}