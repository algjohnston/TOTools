namespace TOTools.Models;

/// <summary>
/// A list of possible games a player could be competing in.
/// </summary>
public enum Game
{
    Ultimate,
    Melee,
    ProjectPlus,
    Rivals,
    RivalsTwo,
    Unknown
}

public static class GameHelper
{
    /// <summary>
    /// Converts an integer to a game enum.
    /// </summary>
    /// <param name="gameInt">The index of the game enum.</param>
    /// <returns>The game enum represented by the given integer.</returns>
    public static Game ConvertToGame(int gameInt)
    {
        if (Enum.IsDefined(typeof(Game), gameInt))
        {
            return (Game)gameInt;
        }

        return Game.Unknown;
    }

    /// <summary>
    /// Converts a game to a string.
    /// </summary>
    /// <param name="game">The game to convert to a string.</param>
    /// <returns>A string representation of the given game.</returns>
    public static string ConvertToString(Game game)
    {
        return game switch
        {
            Game.Ultimate => "Ultimate",
            Game.Melee => "Melee",
            Game.ProjectPlus => "P+",
            Game.Rivals => "Rivals",
            Game.RivalsTwo => "Rivals 2",
            _ => "Unknown Game"
        };
    }
}