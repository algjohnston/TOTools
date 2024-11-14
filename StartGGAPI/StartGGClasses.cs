namespace TOTools.StartGGAPI;

public class EventResponseType()
{
    public EventType Event { get; set; }
}

public class EventType()
{
    public SetConnectionType Sets { get; set; }
}

public class SetType
{
    public string Id { get; set; }
    public string Identifier { get; set; }
    public int Round { get; set; }
    public List<SetSlotType> Slots { get; set; }
    public PhaseGroupType PhaseGroup { get; set; }
}

public class PhaseGroupType
{
    public PhaseType Phase { get; set; }
}

public class PhaseType
{
    public int Id { get; set; }
    public string BracketType { get; set; }
    public int PhaseOrder { get; set; }
}

public class SetSlotType
{
    public EntrantType Entrant { get; set; }
    public string PrereqId { get; set; }
}

public class EntrantType
{
    public string Name { get; set; }
}

public class SetConnectionType
{
    public PageInfoType PageInfo { get; set; }
    public List<SetType> Nodes { get; set; }
}

public class PageInfoType
{
    public int TotalPages { get; set; }
}