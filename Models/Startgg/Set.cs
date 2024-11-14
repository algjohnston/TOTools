using TOTools.StartGGAPI;

namespace TOTools.Models.Startgg;

public class Set
{ 
    private SetType setType;
    public String? nextTopId; 
    public String? nextBottomId;
    public Set(SetType setType)
    {
        this.setType = setType;
        nextTopId = null;
        nextBottomId = null;
    }

    public Set(SetType setType, String nextWinner, String nextLoser)
    {
        this.setType = setType;
        this.nextTopId = nextWinner;
        this.nextBottomId = nextLoser;
    }

    public String GetPrevTopId()
    {
        return setType.Slots.First().PrereqId;
    }

    public String GetPrevBottomId()
    {
        return setType.Slots.Last().PrereqId;
    }

    public String GetId()
    {
        return setType.Id;
    }

    public int GetRound()
    {
        return setType.Round;
    }
    public SetType GetPrevTopSetType() => setType;
    public SetType GetPrevBottomSetType() => setType;
    
}