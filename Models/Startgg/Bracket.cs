
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

    // private void AddNextSets(Set set)
    // {
    //     if (set.GetPrevTop() == null || set.GetPrevBottom() == null) return;
    //     Set topSet = sets[set.GetPrevTop()];
    //     if (topSet.GetRound >= 0)
    //     {
    //         topSet.nextWinner = set.GetPrevBottom
    //     }
    //     topSet.nextWinner = set.GetPrevTop();
    //     topSet.nextLoser = set.GetPrevBottom();
    //     AddNextSets(topSet);
    //     Set bottomSet = sets[set.GetPrevBottom()];
    //     topSet.nextWinner = set.GetPrevTop();
    //     topSet.nextLoser = set.GetPrevBottom();
    // } this was NOT working lmao 

   
}