using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

public class Set
{

    // Startgg variables
    public string Player1Id { get; private set; }
    public string Player2Id { get; private set; }
    public string Player1 { get; private set; }
    public string Player2 { get; private set; }
    public readonly int Round;
    public readonly string DisplayIdentifier;
    public readonly string PrevTopId;
    public readonly string PrevBottomId;
    public readonly string Id;
    public readonly string Identifier;
    private string _winnerId;
    
    // Custom variables
    public Set? NextSet { get; set; }
    public Set? PrevTop { get; set; }
    public Set? PrevBottom { get; set; }
    
    public string? Winner
    {
        get
        {
            if (_winnerId == Player1Id)
            {
                return Player1;
            }

            return _winnerId == Player2Id ? Player2 : null;
        }
        set
        {
            if (value == Player1)
            {
                _winnerId = Player1Id;
            } else if (value == Player2)
            {
                _winnerId = Player2Id;
            }
        }
    }
    
    /// <summary>
    /// A wrapper class for the SetType returned by startgg.
    /// Includes fields to construct a double elimination bracket and allows for player reassignment.
    /// </summary>
    /// <param name="setType">The SetType to wrap.</param>
    public Set(SetType setType)
    {
        var firstSlot = setType.Slots.First();
        var lastSlot = setType.Slots.Last();
        var player1Entrant = firstSlot.Entrant;
        var player2Entrant = lastSlot.Entrant;
        Player1Id = player1Entrant.Id;
        Player2Id = player2Entrant.Id;
        Player1 = player1Entrant.Name;
        Player2 =player2Entrant.Name;
        Round = setType.Round;
        DisplayIdentifier = setType.PhaseGroup.DisplayIdentifier;
        PrevTopId = firstSlot.PrereqId;
        PrevBottomId = lastSlot.PrereqId;
        Id = setType.Id;
        Identifier = setType.Identifier;
        _winnerId = setType.WinnerId;
    }

    public void SwapPlayerWith(Set set)
    {
        (Player1, set.Player1) = (set.Player1, Player1);
        (Player2, set.Player2) = (set.Player2, Player2);
        (Player1Id, set.Player1Id) = (set.Player1Id, Player1Id);
        (Player2Id, set.Player2Id) = (set.Player2Id, Player2Id);
    }
}