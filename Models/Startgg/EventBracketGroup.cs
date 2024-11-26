using System.Collections;
using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

/// <summary>
/// A dictionary of all brackets in a list of phase groups.
/// </summary>
public class EventBracketGroup
{
    private readonly Dictionary<string, Set> _allBracketSets = new(); // Needed to check if prereqs exist

    private readonly Dictionary<string, List<Set>> _roundRobinBrackets = new(); // Every round-robin phase group

    // Every double elimination's phase group winner
    // The bracket sets can be traversed via Set's PrevTop and PrevBottom
    private readonly Dictionary<string, Set> _doubleEliminationWinner = new();

    public EventBracketGroup(List<PhaseGroup> phaseGroups)
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
            var currentSet = phaseGroup.Sets[i++];
            currentRoundRobinSets.Add(new Set(currentSet));
            do
            {
                currentSet = phaseGroup.Sets[i++];
                currentRoundRobinSets.Add(new Set(currentSet));
            } while (!currentSet.Identifier.Equals("A") && i < phaseGroup.Sets.Count);

            _roundRobinBrackets.Add(
                currentRoundRobinSets.First().DisplayIdentifier,
                currentRoundRobinSets);
        }
    }

    /// <summary>
    /// Fills the prerequisite sets recursively and adds the final set to the double elimination sets.
    /// </summary>
    /// <param name="phaseGroup">The phase group that contains the double elimination bracket sets.</param>
    private void AddDoubleEliminationSets(PhaseGroup phaseGroup)
    {
        // Needed to build the set backwards from the winner set
        _allBracketSets.Clear();
        foreach (var set in phaseGroup.Sets)
        {
            _allBracketSets.Add(set.Id, new Set(set));
        }
        
        if (!phaseGroup.PhaseGroupType.Phase.BracketType.Equals("DOUBLE_ELIMINATION"))
        {
            throw new ArgumentException("Invalid phase group type; expected DOUBLE_ELIMINATION");
        }

        var finalWinnerSet = new Set(phaseGroup.Sets.Last());
        _doubleEliminationWinner.Add(finalWinnerSet.DisplayIdentifier, finalWinnerSet);
        FillNextDoubleEliminationBracket(finalWinnerSet);
        _allBracketSets.Clear();
    }

    /// <summary>
    /// Recursively sets the previous sets for a bracket.
    /// I.E. A final set has two prerequisite sets whose winners go to it.
    /// This goes on until the first matches in a bracket.
    /// </summary>
    /// <param name="set">The set to start back-filling at.</param>
    private void FillNextDoubleEliminationBracket(Set set)
    {
        var hasPrevTop = _allBracketSets.ContainsKey(set.PrevTopId);
        if (hasPrevTop)
        {
            var topSet = _allBracketSets[set.PrevTopId];
            topSet.NextSet = set;
            set.PrevTop = topSet;
            FillNextDoubleEliminationBracket(topSet);
        }

        var hasPrevBottom = _allBracketSets.ContainsKey(set.PrevBottomId);
        if (!hasPrevBottom)
        {
            return;
        }

        var bottomSet = _allBracketSets[set.PrevBottomId];
        bottomSet.NextSet = set;
        set.PrevBottom = bottomSet;
        FillNextDoubleEliminationBracket(bottomSet);
    }

    public Dictionary<string, Set> GetSets()
    {
        return _allBracketSets;
    }
}