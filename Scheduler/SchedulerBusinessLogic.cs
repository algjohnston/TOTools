using System.Collections.ObjectModel;
using CS341Project.Models;
using GraphQL;
using GraphQL.Client.Http;
using TOTools.Database;
using TOTools.Models;
using TOTools.Models.Startgg;
using TOTools.StartggAPI;

namespace TOTools.Scheduler;

/// <summary>
/// Business logic for the scheduler
/// </summary>
public class SchedulerBusinessLogic(
    GraphQLHttpClient client,
    MatchTable matchTable // has all matches played previously by any players in any game, used to estimate time
)
{
    public ObservableCollection<EventLink> EventLinks { get; } = [];

    private ObservableCollection<PastMatch> PastMatches => matchTable.SelectAll();

    // This gets filled with matches from startgg that need to be played
    public ObservableCollection<Match> FutureMatches { get; } = [];


    public void AddEvent(EventLink eventLink)
    {
        EventLinks.Add(eventLink);
    }

    private long EstimateMatchLength(string player1, string player2)
    {
        long totalTime = 0;
        var numMatches = 0;

        foreach (var pastMatch in PastMatches)
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
        var numMatches = 0;
        foreach (var pastMatch in PastMatches)
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


    private static bool ArePlayersEqual(string player1, string player2, Match match2)
    {
        return (player1 == match2.Player1 && player2 == match2.Player2) ||
               (match2.Player1 == match2.Player2 && player2 == player1);
    }


    private async Task<List<PhaseGroup>> LoadPotentialMatchList(string url)
    {
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

    private List<Match> GenerateMatchSchedule(List<PhaseGroup> phaseGroups)
    {
        var bracket = new Bracket(phaseGroups);
        
        List<Match> futureMatches = [];

        var matchParticipants = phaseGroups.SelectMany(
            phaseGroup => phaseGroup.Sets.Where(
                set => set.Slots.Count == 2)
        ).ToDictionary(set => set, set => EstimateMatchLength(set.Slots[0].Entrant.Name, set.Slots[1].Entrant.Name));

        var sortedMatchParticipants = matchParticipants
            .OrderByDescending(kv => kv.Value)
            .ToList();

        // TODO get game and isBestOfFive
        futureMatches.AddRange(
            sortedMatchParticipants
                .Select(kvp => new Match(
                    kvp.Key.Slots[0].Entrant.Name,
                    kvp.Key.Slots[1].Entrant.Name,
                    kvp.Value,
                    Game.Unknown,
                    true)));

        return futureMatches;
    }

    public async Task LoadPotentialSchedule()
    {
        foreach (var eventLink in EventLinks.OrderBy(e => e.StartTime))
        {
            var phaseGroups = await LoadPotentialMatchList(eventLink.Link);
            var matches = GenerateMatchSchedule(phaseGroups);
            foreach (var match in matches)
            {
                FutureMatches.Add(match);
            }
        }
    }
    
}