using TOTools.StartggAPI;

namespace TOTools.Models.Startgg;

public class Set
{
    private readonly SetType _setType;
    public Set? NextSet { get; set; }

    public Set(SetType setType)
    {
        _setType = setType;
        NextSet = null;
    }

    public Set(SetType setType, Set nextWinner)
    {
        _setType = setType;
        NextSet = nextWinner;
    }

    public string GetPrevTopId()
    {
        return _setType.Slots.First().PrereqId;
    }

    public string GetPrevBottomId()
    {
        return _setType.Slots.Last().PrereqId;
    }

    public string GetId()
    {
        return _setType.Id;
    }

    public int GetRound()
    {
        return _setType.Round;
    }
    
}