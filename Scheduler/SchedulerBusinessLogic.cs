using System.Collections.ObjectModel;
using CS341Project.Models;
using GraphQL;
using GraphQL.Client.Http;
using TOTools.Database;
using TOTools.Models;
using TOTools.StartGGAPI;

namespace TOTools.Scheduler;

/// <summary>
/// Business logic for the scheduler
/// </summary>
public class SchedulerBusinessLogic
{
    // this has all matches played previously by any players in any game, used to estimate time
    private readonly ITable<PastMatch, long, Match> _table = new MatchTable();

    private readonly GraphQLHttpClient startGGClient;

    public SchedulerBusinessLogic(GraphQLHttpClient client)
    {
        startGGClient = client;
    }

    private ObservableCollection<PastMatch> PastMatches => _table.SelectAll();

    // This gets filled with matches from startgg that need to be played
    public ObservableCollection<Match> FutureMatches { get; } = [];


    public long EstimateMatchLength(string player1, string player2)
    {
        long totalTime = 0;
        int numMatches = 0;

        foreach (PastMatch pastMatch in PastMatches)
        {
            if (ArePlayersEqual(player1, player2, pastMatch)
                // && AreMatchesComparable(player1, pastMatch, pastMatch)
               )
            {
                totalTime += pastMatch.TimeInSeconds;
                numMatches++;
            }
        }

        // if the players have never played, will give the average match length from all matches
        return (numMatches != 0) ? totalTime / numMatches : GetAverageMatchLength();
    }

    private long GetAverageMatchLength()
    {
        long totalTime = 0;
        int numMatches = 0;
        foreach (PastMatch pastMatch in PastMatches)
        {
            // if (AreMatchesComparable(match, pastMatch))
            // {
            totalTime += pastMatch.TimeInSeconds;
            numMatches++;
            // }
        }

        // TODO has to change to 0
        // this will return -1 if there is nothing in the table for previous matches, but this should basically never happen
        return (numMatches != 0) ? totalTime / numMatches : 0;
    }


    private static bool AreMatchesComparable(Match match1, PastMatch match2)
    {
        // matches are comparable if the games are the same, and they are either both best of 5, or best of 3
        return (match1.IsBestOfFive == match2.IsBestOfFive) && (match1.GameName.Equals(match2.GameName));
    }


    private bool ArePlayersEqual(string player1, string player2, Match match2)
    {
        return (player1 == match2.Player1 && player2 == match2.Player2) ||
               (match2.Player1 == match2.Player2 && player2 == player1);
    }


    private async Task<List<PhaseGroup>> LoadPotentialMatchList(string url)
    {
        var currentPage = 1;
        var responses = new List<GraphQLResponse<EventResponseType>>();
        var graphQLResponse = await startGGClient.SendQueryAsync<EventResponseType>(
            StartGGQueries.CreateEventSetsQuery(url, currentPage));
        responses.Add(graphQLResponse);
        while (currentPage < graphQLResponse.Data.Event.Sets.PageInfo.TotalPages)
        {
            currentPage++;
            graphQLResponse = await startGGClient.SendQueryAsync<EventResponseType>(
                StartGGQueries.CreateEventSetsQuery(url, currentPage));
            responses.Add(graphQLResponse);
        }
        
        Dictionary<int, List<SetType>> phaseGroupLists = [];
        Dictionary<int, PhaseGroupType> phaseGroupTypes = [];

        foreach (var qlResponse in responses)
        {
            var eventType = qlResponse.Data.Event;
            var nodes = eventType.Sets.Nodes;

            foreach (var node in nodes)
            {
                var phaseGroup = node.PhaseGroup;
                var phaseOrder = phaseGroup.Phase.PhaseOrder;
                if (!phaseGroupLists.TryGetValue(phaseOrder, out List<SetType>? value))
                {
                    value = [];
                    phaseGroupLists.Add(phaseOrder, value);
                    phaseGroupTypes.Add(phaseOrder, phaseGroup);
                }

                value.Add(node);
            }
        }

        var phaseGroups = phaseGroupTypes
            .OrderBy(pg => pg.Key)
            .Select(pg =>
                new PhaseGroup(
                    pg.Value,
                    phaseGroupLists[pg.Key]
                        .OrderBy(set => set.Round)
                        .ThenBy(set => set.Identifier)
                        .ToList()
                ))
            .ToList();
        
        return phaseGroups;
    }

    private List<Match> GenerateMatchSchedule(List<PhaseGroup> phaseGroups)
    {
        List<Match> futureMatches = [];

        var matchParticipants = new Dictionary<SetType, long>();
        foreach (var phaseGroup in phaseGroups)
        {
            foreach (var set in phaseGroup.Sets)
            {
                if (set.Slots.Count == 2)
                {
                    matchParticipants.Add(set, EstimateMatchLength(set.Slots[0].Entrant.Name, set.Slots[1].Entrant.Name));
                }
            }
        }

        var sortedMatchParticipants = matchParticipants
            .OrderByDescending(kv => kv.Value)
            .ToList();

        foreach (var kvp in sortedMatchParticipants)
        {
            // TODO bo3 or 5, and game
            futureMatches.Add(
                new Match(
                    kvp.Key.Slots[0].Entrant.Name,
                    kvp.Key.Slots[1].Entrant.Name,
                    kvp.Value,
                    Game.Unknown,
                    true)
            );
        }

        return futureMatches;
    }

    public async void LoadPotentialSchedule(IList<EventLink> events)
    {
        foreach (var event_ in events.OrderBy(e => e.StartTime))
        {
            var phaseGroups = await LoadPotentialMatchList(event_.Link);
            var matches = GenerateMatchSchedule(phaseGroups);
            foreach (var match in matches)
            {
                FutureMatches.Add(match);
            }
        }
    }
}