
using TOTools.StartGGAPI;

namespace TOTools.Models.Startgg;

public class Bracket
{
    Dictionary<String, Set> sets = new();

    public Bracket(List<PhaseGroup> phaseGroups)
    {
        //creates the grand finals set
        Set grandFinals = new Set(phaseGroups.Last().Sets.Last());

        foreach (PhaseGroup pg in phaseGroups)
        {
            foreach (SetType setType in pg.Sets)
            {
                Set curSet = new Set(setType);
                sets.Add(curSet.GetId(), curSet);
            }
        }

        // AddNextSets(grandFinals);
    }

    private void AddNextSets(Set set)
    {
        if (set.GetPrevTopId() == null || set.GetPrevBottomId() == null) return;
        Set topSet = sets[set.GetPrevTopId()];
        Set bottomSet = sets[set.GetPrevBottomId()];
        topSet.nextTopId = set.GetPrevTopId();
        topSet.nextBottomId = set.GetPrevBottomId();
        AddNextSets(topSet);
        AddNextSets(bottomSet);
    }
}