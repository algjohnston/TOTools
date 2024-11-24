using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

public class Set(SetType setType)
{
    public Set? NextSet { get; set; } = null;

    public string DisplayIdentifier => setType.PhaseGroup.DisplayIdentifier;
    
    public string PrevTopId => setType.Slots.First().PrereqId;

    public string PrevBottomId => setType.Slots.Last().PrereqId;

    public string Id => setType.Id;
    
}