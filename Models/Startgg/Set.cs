using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

/// <summary>
/// A wrapper class for the SetType returned by startgg.
/// Includes fields to construct a double elimination bracket and allows for player reassignment.
/// </summary>
/// <param name="setType">The SetType to wrap.</param>
public class Set(SetType setType)
{
    public string Player1 { get; set; } = setType.Slots.First().Entrant.Name;
    public string Player2 { get; set; } = setType.Slots.Last().Entrant.Name;
    public int Round { get; } = setType.Round;
    public Set? NextSet { get; set; } = null;

    public Set? PrevTop { get; set; } = null;

    public Set? PrevBottom { get; set; } = null;

    public string DisplayIdentifier => setType.PhaseGroup.DisplayIdentifier;

    public string PrevTopId => setType.Slots.First().PrereqId;

    public string PrevBottomId => setType.Slots.Last().PrereqId;

    public string Id => setType.Id;
    
    public string Identifier => setType.Identifier;

    public void SwapPlayerWith(Set set)
    {
        (Player1, set.Player1) = (set.Player1, Player1);
        (Player2, set.Player2) = (set.Player2, Player2);
    }
}