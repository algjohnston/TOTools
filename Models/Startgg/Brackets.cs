using System.Collections;
using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

/// <summary>
/// A dictionary of all brackets in a list of phase groups.
/// </summary>
public class Brackets
{
    private readonly Dictionary<string, Set> _allBracketSets = new(); // Needed to check if prereqs exist

    private readonly Dictionary<string, List<Set>> _roundRobinBrackets = new();
    private readonly Dictionary<string, Set> _doubleEliminationWinner = new();

    public Brackets(List<PhaseGroup> phaseGroups)
    {
        foreach (var phaseGroup in phaseGroups)
        {
            var bracketType = phaseGroup.PhaseGroupType.Phase.BracketType;
            switch (bracketType)
            {
                case "ROUND_ROBIN":
                {
                    AddRoundRobinSets(phaseGroup);
                    break;
                }
                case "DOUBLE_ELIMINATION":
                {
                    AddDoubleEliminationSets(phaseGroup);
                    break;
                }
            }
        }
    }

    public List<List<Set>> GetRoundRobinBrackets()
    {
        return _roundRobinBrackets.Values.ToList();
    }

    public List<Set> GetDoubleEliminationLoserWinner()
    {
        return _doubleEliminationWinner.Values.ToList();
    }

    /// <summary>
    /// Goes through all the round-robin brackets,
    /// makes a list of all sets that comprise each one,
    /// and adds them to the round-robin brackets.
    /// </summary>
    /// <param name="phaseGroup">The phase group that contains the round-robin brackets.</param>
    private void AddRoundRobinSets(PhaseGroup phaseGroup)
    {
        var i = 0;
        while (i < phaseGroup.Sets.Count)
        {
            List<Set> currentRoundRobinSets = []; 
            SetType currentSet; // Needed to compile
            do
            {
                currentSet = phaseGroup.Sets[i];
                currentRoundRobinSets.Add(new Set(currentSet));
                i++;
            } while (!currentSet.Id.Equals("A") && i < phaseGroup.Sets.Count);

            _roundRobinBrackets.Add(
                phaseGroup.PhaseGroupType.DisplayIdentifier,
                currentRoundRobinSets);
        }
    }

    /// <summary>
    /// Fills the prerequisite sets recursively and adds the final set to the double elimination sets.
    /// </summary>
    /// <param name="phaseGroup">The phase group that contains the double elimination bracket sets.</param>
    private void AddDoubleEliminationSets(PhaseGroup phaseGroup)
    {
        if (!phaseGroup.PhaseGroupType.Phase.BracketType.Equals("DOUBLE_ELIMINATION"))
        {
            throw new ArgumentException("Invalid phase group type; expected DOUBLE_ELIMINATION");
        }
        
        var finalWinnerSet = new Set(phaseGroup.Sets.Last());
        _doubleEliminationWinner.Add(finalWinnerSet.Id, finalWinnerSet);
        FillNextDoubleEliminationBracket(finalWinnerSet);
    }

    /// <summary>
    /// Recursively sets the previous sets for a bracket.
    /// I.E. A final set has two prerequisite sets whose winners go to it.
    /// This goes on until the first matches in a bracket.
    /// </summary>
    /// <param name="set">The set to start back-filling at.</param>
    private void FillNextDoubleEliminationBracket(Set set)
    {
        _allBracketSets.Add(set.Id, set);
        var hasNextTop = _allBracketSets.ContainsKey(set.PrevTopId);
        var hasNextBottom = _allBracketSets.ContainsKey(set.PrevBottomId);
        if (hasNextTop)
        {
            var topSet = _allBracketSets[set.PrevTopId];
            topSet.NextSet = set;
            FillNextDoubleEliminationBracket(topSet);
        }

        if (!hasNextBottom)
        {
            return;
        }

        var bottomSet = _allBracketSets[set.PrevBottomId];
        bottomSet.NextSet = set;
        FillNextDoubleEliminationBracket(bottomSet);
    }

    public Dictionary<string, Set> GetSets()
    {
        return _allBracketSets;
    }
}