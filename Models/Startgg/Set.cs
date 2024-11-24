using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

/// <summary>
/// A wrapper class for the SetType returned by startgg.
/// Includes fields to construct a double elimination bracket.
/// </summary>
/// <param name="setType"></param>
public class Set(SetType setType)
{
    public Set? NextSet { get; set; } = null;

    public Set? PrevTop { get; set; } = null;

    public Set? PrevBottom { get; set; } = null;

    public string DisplayIdentifier => setType.PhaseGroup.DisplayIdentifier;

    public string PrevTopId => setType.Slots.First().PrereqId;

    public string PrevBottomId => setType.Slots.Last().PrereqId;

    public string Id => setType.Id;
}