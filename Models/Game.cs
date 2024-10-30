namespace CS341Project.Models;
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
    public static Game ConvertToGame(int gameInt)
    {
        if (Enum.IsDefined(typeof(Game), gameInt))
        {
            return (Game)gameInt;
        }
        return Game.Unknown;
    }

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
