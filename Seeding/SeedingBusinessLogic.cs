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
    
    public void LoadPlayersFromEntrants()
    {
        //TODO
        /* load player IDS from entrant list gotten from query of the event
         *
         * check if their IDs are in the seeding list, if not, prompt with edit player popup
         * with tag param being their tag in startgg, region as null, and tier / ranking as null
         */
    }

    public void SeedPlayerList(List<Player> players)
    {
        // this is all seeding a list of players will be, since we made them comparable based on tier and ranking
        players.Sort();
    }

    public void SeedEvent()
    {
        //TODO
        /* I don't really know how we do this with a local bracket data structure. I think startgg's phaseGroup stuff haas something for seed numbers in it that
           will let us edit who goes where locally. If so, we just iterate through the bracket and replace whatever player IDs are their with the player ID from our
           list whose seed should go there. (the list won't have actual seed info, it's just ordered in order of seed, so position 0 is seed 1, pos 1 is seed 2, etc.)
           We then take that data structure and use it in our bracket viewable. Moving players around in their will have to swap their positions in the list of players ordered by
           seeding as well, since that is what we're inevitably gonna push to startgg, or show them so they can input it manually into startgg*/
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