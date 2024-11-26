using TOTools.Models.Startgg;

namespace TOTools.Models;

/// <summary>
/// A bracket for use by the bracket editor.
/// To swap seeds, change players, and submit match results.
/// </summary>
/// <param name="bracketType">The BracketType of the bracket.</param>
/// <param name="sets">The list of sets that make up the bracket.</param>
public class Bracket(BracketType bracketType, List<Set> sets)
{
    public BracketType BracketType { get; } = bracketType;
    private List<Set> Sets { get; } = sets;

    public static WinnerAndLoserBracketSets GetWinnerAndLoserBracketSets(Bracket doubleEliminationBracket)
    {
        HashSet<Set> winners = [];
        HashSet<Set> losers = [];

        var winner = doubleEliminationBracket.Sets.First();
        winners.Add(winner);
        List<Set> currentSets = [winner];
        while (currentSets.Count > 0)
        {
            var currentSet = currentSets.First();
            currentSets.RemoveAt(0);
            if (currentSet.PrevTop != null)
            {
                if (currentSet.PrevTop.Round >= 0)
                {
                    winners.Add(currentSet.PrevTop);
                }
                else
                {
                    losers.Add(currentSet.PrevTop);
                }

                currentSets.Add(currentSet.PrevTop);
            }

            if (currentSet.PrevBottom == null) continue;
            if (currentSet.PrevBottom.Round >= 0)
            {
                winners.Add(currentSet.PrevBottom);
            }
            else
            {
                losers.Add(currentSet.PrevBottom);
            }

            currentSets.Add(currentSet.PrevBottom);
        }

        var winnersSets = winners.ToList();
        winnersSets.Sort((set1, set2) =>
        {
            var roundComparison = set1.Round.CompareTo(set2.Round);
            return roundComparison != 0 ? roundComparison : string.Compare(set1.Identifier, set2.Identifier, StringComparison.Ordinal);
        });

        var loserSets = losers.ToList();
        loserSets.Sort((set1, set2) =>
        {
            var roundComparison = set1.Round.CompareTo(set2.Round);
            return roundComparison != 0 ? roundComparison : string.Compare(set1.Identifier, set2.Identifier, StringComparison.Ordinal);
        });

        return new WinnerAndLoserBracketSets(winnersSets, loserSets);
    }
}

public record WinnerAndLoserBracketSets(List<Set> WinnerSets, List<Set> LoserSets);