using System.Collections.ObjectModel;
using GraphQL;
using GraphQL.Client.Http;
using TOTools.Database;
using TOTools.Models;
using TOTools.Models.Startgg;
using TOTools.StartggAPI;

namespace TOTools.Seeding;

/// <summary>
/// The seeding business logic.
/// </summary>
/// <param name="client">The startgg graphQL client.</param>
/// <param name="playerTable">The player table in the database.</param>
public class SeedingBusinessLogic(
    GraphQLHttpClient client,
    PlayerTable playerTable)
{
    public ObservableCollection<Player> Players { get; } = [];

    public List<EventBracketGroup> EventBrackets { get; } = [];

    private readonly TaskCompletionSource<bool> _loadCompletionSource = new();
    public Task PlayerLoadTask => _loadCompletionSource.Task;

    /// <summary>
    /// Load the players from the player table in the database.
    /// Called at app start.
    /// </summary>
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

    /// <summary>
    /// Adds an event bracket group to be used by the bracket editor.
    /// </summary>
    /// <param name="eventBracketGroup">The event bracket group to add.</param>
    public void AddBracketGroup(EventBracketGroup eventBracketGroup)
    {
        EventBrackets.Add(eventBracketGroup);
    }

    /// <summary>
    /// Takes a startgg event link and adds its brackets.
    /// </summary>
    /// <param name="linkText">The event link.</param>
    public async Task AddLinkPhaseGroups(string linkText)
    {
        var link = EventLink.ExtractTournamentPath(linkText);
        var phaseGroups = await LoadPhaseGroups(link);
        var bracket = new EventBracketGroup(phaseGroups);
        AddBracketGroup(bracket);
    }

    /// <summary>
    /// Loads all the phase groups in an event specified by a startgg event link.
    /// </summary>
    /// <param name="url">The url of the event.</param>
    /// <returns>The phase groups of the event.</returns>
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