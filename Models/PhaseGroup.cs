using TOTools.StartggAPI;

namespace TOTools.Models;

public class PhaseGroup
{
    public PhaseGroupType PhaseGroupType { get; }
    public List<SetType> Sets { get; }
    
    public PhaseGroup(PhaseGroupType phaseGroupType, List<SetType> sets)
    {
        PhaseGroupType = phaseGroupType;
        Sets = sets;
    }
}