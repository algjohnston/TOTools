namespace TOTools.Models.Startgg;

public enum TournamentType
{
    Weekly,
    Monthly,
    Regional,
    Major,
    Unknown
}

public static class TournamentTypeHelper
{
     public static string ToString(TournamentType tourneyType)
    {
        return tourneyType switch
        {
            TournamentType.Weekly => "Weekly",
            TournamentType.Monthly => "Monthly",
            TournamentType.Regional => "Regional",
            TournamentType.Major => "Major",
            TournamentType.Unknown => "Unknown",
            _ => throw new ArgumentOutOfRangeException(nameof(TournamentType), tourneyType, null)
        };
    }
}