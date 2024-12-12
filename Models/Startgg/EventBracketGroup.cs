using System.Collections;
using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

/// <summary>
/// A dictionary of all brackets in a list of phase groups.
/// </summary>
public class EventBracketGroup
{
    private readonly Dictionary<string, Set> _allBracketSets = new();

    private readonly Dictionary<string, List<Set>> _roundRobinBrackets = new();

    // Every double elimination's phase group winner
    // The bracket sets can be traversed via Set's PrevTop and PrevBottom
    private readonly Dictionary<string, List<Set>> _doubleEliminationWinners = new();

    public string EventName { get; private set; }

    public EventBracketGroup(string eventName, List<PhaseGroup> phaseGroups)
    {
        EventName = eventName;
        foreach (var phaseGroup in phaseGroups)
        {
            foreach (var set in phaseGroup.Sets)
            {
                _allBracketSets.TryAdd(set.Id, new Set(set));
            }
        }

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
                currentRoundRobinSets.Add(_allBracketSets[currentSet.Id]);
                currentSet = phaseGroup.Sets[i++];
            } while (!currentSet.Identifier.Equals("A") && i < phaseGroup.Sets.Count);

            var displayIdentifier = currentRoundRobinSets.First().DisplayIdentifier;
            _roundRobinBrackets.Add(displayIdentifier, currentRoundRobinSets);
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

        Dictionary<string, Set> bracketSets = new Dictionary<string, Set>();
        foreach (var set in phaseGroup.Sets)
        {
            var wrappedSet = _allBracketSets[set.Id];
            bracketSets[wrappedSet.Id] = wrappedSet;
            FillDoubleEliminationSet(wrappedSet);
        }

        List<Set> finalWinnerSets = [];
        foreach (var set in bracketSets.Values)
        {
            if (set.NextSet == null)
            {
                finalWinnerSets.Add(_allBracketSets[set.Id]);
            }
        }
        
        _doubleEliminationWinners.Add(
            finalWinnerSets.First().DisplayIdentifier,
            finalWinnerSets
        );
    }

    /// <summary>
    /// Sets the previous and next sets of a given set.
    /// </summary>
    /// <param name="set">The set to fill.</param>
    private void FillDoubleEliminationSet(Set set)
    {
        var hasPrevTop = _allBracketSets.ContainsKey(set.PrevTopId);
        if (hasPrevTop)
        {
            var topSet = _allBracketSets[set.PrevTopId];
            topSet.NextSet = set;
            set.PrevTop = topSet;
        }

        var hasPrevBottom = _allBracketSets.ContainsKey(set.PrevBottomId);
        if (!hasPrevBottom)
        {
            return;
        }

        var bottomSet = _allBracketSets[set.PrevBottomId];
        bottomSet.NextSet = set;
        set.PrevBottom = bottomSet;
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