using System.Collections.ObjectModel;
using GraphQL;
using GraphQL.Client.Http;
using TOTools.Database;
using TOTools.Models;
using TOTools.Models.Startgg;
using TOTools.StartggAPI;

namespace TOTools.Seeding;

public class SeedingBusinessLogic(
    GraphQLHttpClient client,
    PlayerTable playerTable)
{
    public ObservableCollection<Player> Players { get; } = [];
    
    public Brackets? Brackets { get; private set; }
    
    private readonly TaskCompletionSource<bool> _loadCompletionSource = new();
    public Task LoadTask => _loadCompletionSource.Task;
    
    public void LoadPlayers()
    {
        var players = playerTable.SelectAll();
        Players.Clear();
        foreach (var player in players)
        {
            Players.Add(player);   
        }
        _loadCompletionSource.TrySetResult(true);
    }

    public void SetBracket(Brackets brackets)
    {
        Brackets = brackets;
    }

    public async Task AddLink(string linkText)
    {
        var link = EventLink.ExtractTournamentPath(linkText);
        var phaseGroups = await LoadPhaseGroups(link);
        var bracket = new Brackets(phaseGroups);
        SetBracket(bracket);
    }
    
    public async Task<List<PhaseGroup>> LoadPhaseGroups(string url)
    {
        // Load all the phase groups across all the online pages for an event
        var currentPage = 1;
        var responses = new List<GraphQLResponse<EventResponseType>>();
        var graphQLResponse = await client.SendQueryAsync<EventResponseType>(
            StartGGQueries.CreateEventSetsQuery(url, currentPage));
        responses.Add(graphQLResponse);
        while (currentPage < graphQLResponse.Data.Event.Sets.PageInfo.TotalPages)
        {
            currentPage++;
            graphQLResponse = await client.SendQueryAsync<EventResponseType>(
                StartGGQueries.CreateEventSetsQuery(url, currentPage));
            responses.Add(graphQLResponse);
        }

        // Add all the sets and phase group types to their corresponding phase group number
        Dictionary<int, List<SetType>> phaseGroupSetLists = [];
        Dictionary<int, PhaseGroupType> phaseGroupTypes = [];
        foreach (var qlResponse in responses)
        {
            var eventType = qlResponse.Data.Event;
            var setTypes = eventType.Sets.Nodes;

            foreach (var setType in setTypes)
            {
                var phaseGroup = setType.PhaseGroup;
                var phaseOrder = phaseGroup.Phase.PhaseOrder;
                if (!phaseGroupSetLists.TryGetValue(phaseOrder, out var value))
                {
                    value = [];
                    phaseGroupSetLists.Add(phaseOrder, value);
                    phaseGroupTypes.Add(phaseOrder, phaseGroup);
                }

                value.Add(setType);
            }
        }

        // Order the phase group by the phase order, then order the set by
        // their phase group's display identifier, which round they are, and then their id
        var phaseGroups = phaseGroupTypes
            .OrderBy(pg => pg.Key)
            .Select(pg =>
                new PhaseGroup(
                    pg.Value,
                    phaseGroupSetLists[pg.Key]
                        .OrderBy(set => set.PhaseGroup.DisplayIdentifier)
                        .ThenBy(set => set.Round)
                        .ThenBy(set => set.Identifier)
                        .ToList()
                ))
            .ToList();

        return phaseGroups;
    }
}