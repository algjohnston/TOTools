namespace TOTools.Models.Startgg;

public class Bracket
{
    private readonly Dictionary<string, Set> _sets = new();

    public Bracket(List<PhaseGroup> phaseGroups)
    {
        //creates the grand finals set
        var grandFinals = new Set(phaseGroups.Last().Sets.Last());

        foreach (var currentSet in phaseGroups.SelectMany(phaseGroup =>
                     phaseGroup.Sets.Select(setType => new Set(setType))))
        {
            _sets.Add(currentSet.GetId(), currentSet);
        }

        AddNextSets(grandFinals);
    }

    private void AddNextSets(Set set)
    {
        var haveNextTop = _sets.ContainsKey(set.GetPrevTopId());
        var haveNextBottom = _sets.ContainsKey(set.GetPrevBottomId());
        if (haveNextTop)
        {
            var topSet = _sets[set.GetPrevTopId()];
            topSet.NextSet = set;
            AddNextSets(topSet);
        }

        if (!haveNextBottom)
        {
            return;
        }
        var bottomSet = _sets[set.GetPrevBottomId()];
        bottomSet.NextSet = set;
        AddNextSets(bottomSet);
    }
    
}