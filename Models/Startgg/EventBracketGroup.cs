using System.Collections;
using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

/// <summary>
/// A dictionary of all brackets in a list of phase groups.
/// </summary>
public class EventBracketGroup
{
    private readonly Dictionary<string, Set> _allBracketSets = new();
    
    private readonly Dictionary<string, Set> _doubleEliminationBracketSets = new();

    private readonly Dictionary<string, List<Set>> _roundRobinBrackets = new(); // Every round-robin phase group

    // Every double elimination's phase group winner
    // The bracket sets can be traversed via Set's PrevTop and PrevBottom
    private readonly Dictionary<string, List<Set>> _doubleEliminationWinners = new();

    public string EventName { get; private set; }
    
    public EventBracketGroup(string eventName, List<PhaseGroup> phaseGroups)
    {
        EventName = eventName;
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

    public List<List<Set>> GetDoubleEliminationWinners()
    {
        return _doubleEliminationWinners.Values.ToList();
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
            do
            {
                currentRoundRobinSets.Add(new Set(currentSet));
                currentSet = phaseGroup.Sets[i++];
            } while (!currentSet.Identifier.Equals("A") && i < phaseGroup.Sets.Count);

            var displayIdentifier = currentRoundRobinSets.First().DisplayIdentifier;
            _roundRobinBrackets.Add(displayIdentifier, currentRoundRobinSets);
            foreach (var set in currentRoundRobinSets)
            {
                _allBracketSets.Add(set.Id, set);
            }
        }
    }

    /// <summary>
    /// Fills the prerequisite sets recursively and adds the final set to the double elimination sets.
    /// </summary>
    /// <param name="phaseGroup">The phase group that contains the double elimination bracket sets.</param>
    private void AddDoubleEliminationSets(PhaseGroup phaseGroup)
    {
        // Find all the final winner sets

        
        // Needed to build the set backwards from the final winner sets
        _doubleEliminationBracketSets.Clear();
        foreach (var set in phaseGroup.Sets)
        {
            _doubleEliminationBracketSets.Add(set.Id, new Set(set));
        }
        
        if (!phaseGroup.PhaseGroupType.Phase.BracketType.Equals("DOUBLE_ELIMINATION"))
        {
            throw new ArgumentException("Invalid phase group type; expected DOUBLE_ELIMINATION");
        }

        List<string> preRequisiteSets = [];
        foreach(var set in phaseGroup.Sets)
        {
            preRequisiteSets.Add(set.Slots.First().PrereqId);
            preRequisiteSets.Add(set.Slots.Last().PrereqId);
        }

        List<Set> finalWinnerSets = [];
        foreach (var set in phaseGroup.Sets)
        {
            if (!preRequisiteSets.Contains(set.Id))
            {
                finalWinnerSets.Add(new Set(set));
            }
        }
        
        _doubleEliminationWinners.Add(
            finalWinnerSets.First().DisplayIdentifier,
            finalWinnerSets
            );
        foreach (var set in finalWinnerSets)
        {
            FillNextDoubleEliminationBracket(set);
        }
        _doubleEliminationBracketSets.Clear();
    }

    /// <summary>
    /// Recursively sets the previous sets for a bracket.
    /// I.E. A final set has two prerequisite sets whose winners go to it.
    /// This goes on until the first matches in a bracket.
    /// </summary>
    /// <param name="set">The set to start back-filling at.</param>
    private void FillNextDoubleEliminationBracket(Set set)
    {
        _allBracketSets.TryAdd(set.Id, set);
        var hasPrevTop = _doubleEliminationBracketSets.ContainsKey(set.PrevTopId);
        if (hasPrevTop)
        {
            var topSet = _doubleEliminationBracketSets[set.PrevTopId];
            topSet.NextSet = set;
            set.PrevTop = topSet;
            FillNextDoubleEliminationBracket(topSet);
        }

        var hasPrevBottom = _doubleEliminationBracketSets.ContainsKey(set.PrevBottomId);
        if (!hasPrevBottom)
        {
            return;
        }

        var bottomSet = _doubleEliminationBracketSets[set.PrevBottomId];
        bottomSet.NextSet = set;
        set.PrevBottom = bottomSet;
        FillNextDoubleEliminationBracket(bottomSet);
    }

    public Dictionary<string, Set> GetSets()
    {
        return _allBracketSets;
    }

    public Match? RecordWinner(string winner, string id)
    {
        _allBracketSets[id].Winner = winner;
        var nextSet = _allBracketSets[id].NextSet;
        if (nextSet == null)
        {
            return null;
        }

        return null;

    }
}