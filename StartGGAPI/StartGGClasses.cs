namespace TOTools.StartggAPI;

/**
 * A list of classes used by the GraphQL.Client API to load the startgg API query responses.
 * These classes must match the startgg graphql queries for the GraphQL.Client to work.
 */
public class EventResponseType()
{
    public required EventType Event { get; set; }
}

public class EventType()
{
    public required SetConnectionType Sets { get; set; }
}

public class SetType
{
    public required string Id { get; set; }
    public required string Identifier { get; set; }
    public int Round { get; set; }
    public required List<SetSlotType> Slots { get; set; }
    public required PhaseGroupType PhaseGroup { get; set; }
}

public class PhaseGroupType
{
    public required string DisplayIdentifier { get; set; }
    public required PhaseType Phase { get; set; }
}

public class PhaseType
{
    public int Id { get; set; }
    public required string BracketType { get; set; }
    public int PhaseOrder { get; set; }
}

public class SetSlotType
{
    public required EntrantType Entrant { get; set; }
    public required string PrereqId { get; set; }
}

public class EntrantType
{
    public required string Name { get; set; }
}

public class SetConnectionType
{
    public required PageInfoType PageInfo { get; set; }
    public required List<SetType> Nodes { get; set; }
}

public class PageInfoType
{
    public int TotalPages { get; set; }
}