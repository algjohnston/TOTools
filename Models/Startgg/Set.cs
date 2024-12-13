using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

public class Set
{
    // Startgg variables
    public string? Player1Id { get; private set; }
    public string? Player2Id { get; private set; }

    public string? Player1DisplayName { get; private set; }
    public string? Player2DisplayName { get; private set; }

    private string? _player1Tag;

    public string? Player1Tag
    {
        get => _player1Tag;
        private set
        {
            if (value == null)
            {
                return;
            }
            _player1Tag = value;
            Player1DisplayName = _player1Tag[(!_player1Tag.Contains('|') ? 0 : _player1Tag.IndexOf('|') + 1)..];
        }
    }

    private string? _player2Tag;

    public string? Player2Tag
    {
        get => _player2Tag;
        private set
        {
            if (value == null)
            {
                return;
            }
            _player2Tag = value;
            Player2DisplayName = _player2Tag[(!_player2Tag.Contains('|') ? 0 : _player2Tag.IndexOf('|') + 1)..];
        }
    }  
    
    public readonly int Round;
    public readonly string DisplayIdentifier;
    public readonly string ActualDisplayIdentifier;
    public readonly string PrevTopId;
    public readonly string PrevBottomId;
    public readonly string Id;
    public readonly string Identifier;
    private string? _winnerId;

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
                return Player1Tag;
            }

            return _winnerId == Player2Id ? Player2Tag : null;
        }
        set
        {
            if (value == Player1Tag)
            {
                _winnerId = Player1Id;
            }
            else if (value == Player2Tag)
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
        Player1Id = player1Entrant?.Id;
        Player2Id = player2Entrant?.Id;
        Player1Tag = player1Entrant?.Name;
        Player2Tag = player2Entrant?.Name;
        if (Player1Tag != null)
        {
            Player1DisplayName = Player1Tag[(Player1Tag.IndexOf('|') == -1 ? 0 : Player1Tag.IndexOf('|') + 1)..];
        }

        if (Player2Tag != null)
        {
            Player2DisplayName = Player2Tag[(Player2Tag.IndexOf('|') == -1 ? 0 : Player2Tag.IndexOf('|') + 1)..];
        }

        Round = setType.Round;
        DisplayIdentifier = setType.PhaseGroup.DisplayIdentifier + ": Phase #" + setType.PhaseGroup.Phase.PhaseOrder;
        ActualDisplayIdentifier = setType.PhaseGroup.DisplayIdentifier;
        PrevTopId = firstSlot.PrereqId;
        PrevBottomId = lastSlot.PrereqId;
        Id = setType.Id;
        Identifier = setType.Identifier;
        _winnerId = setType.WinnerId;
    }

    public void SwapPlayerWith(Set set)
    {
        (Player1Tag, set.Player1Tag) = (set.Player1Tag, Player1Tag);
        (Player2Tag, set.Player2Tag) = (set.Player2Tag, Player2Tag);
        (Player1Id, set.Player1Id) = (set.Player1Id, Player1Id);
        (Player2Id, set.Player2Id) = (set.Player2Id, Player2Id);
    }
}