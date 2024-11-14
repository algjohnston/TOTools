using TOTools.StartGGAPI;

namespace TOTools.Models.Startgg;

public class Set
{ 
    private SetType setType;
    public String? nextWinner; 
    public String? nextLoser;
    public Set(SetType setType)
    {
        this.setType = setType;
        nextWinner = null;
        nextLoser = null;
    }

    public Set(SetType setType, String nextWinner, String nextLoser)
    {
        this.setType = setType;
        this.nextWinner = nextWinner;
        this.nextLoser = nextLoser;
    }

    public String GetPrevTop()
    {
        return setType.Slots.First().PrereqId;
    }

    public String GetPrevBottom()
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