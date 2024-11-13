using System.Collections.ObjectModel;
using GraphQL.Client.Abstractions;
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
    private readonly ITable<Match, long> _table = new MatchTable();

    private readonly GraphQLHttpClient startGGClient;

    public SchedulerBusinessLogic(GraphQLHttpClient client)
    {
        startGGClient = client;
    }

    public ObservableCollection<Match> pastMatches => _table.SelectAll();

    // This gets filled with matches from startgg that need to be played
    public ObservableCollection<Match> CurrentMatches { get; } = [];


    public long EstimateMatchLength(Match match)
    {
        long totalTime = 0;
        int numMatches = 0;

        foreach (Match pastMatch in pastMatches)
        {
            if (ArePlayersEqual(pastMatch, match) && AreMatchesComparable(match, pastMatch))
            {
                totalTime += pastMatch.TimeInSeconds;
                numMatches++;
            }
        }

        // if the players have never played, will give the average match length from all matches
        return (numMatches != 0) ? totalTime / numMatches : GetAverageMatchLength(match);
    }

    public long GetAverageMatchLength(Match match)
    {
        long totalTime = 0;
        int numMatches = 0;
        foreach (Match pastMatch in pastMatches)
        {
            if (AreMatchesComparable(match, pastMatch))
            {
                totalTime += pastMatch.TimeInSeconds;
                numMatches++;
            }
        }

        // this will return -1 if there is nothing in the table for previous matches, but this should basically never happen
        return (numMatches != 0) ? totalTime / numMatches : -1;
    }


    private bool AreMatchesComparable(Match match1, Match match2)
    {
        // matches are comperable if the games are the same, and they are either both best of 5, or best of 3
        return (match1.isBestOfFive == match2.isBestOfFive) && (match1.GameName.Equals(match2.GameName));
    }


    private bool ArePlayersEqual(Match match1, Match match2)
    {
        return (match1.Player1 == match2.Player1 && match1.Player2 == match2.Player2) ||
               (match2.Player1 == match2.Player2 && match1.Player2 == match1.Player1);
    }


    public async Task<List<PhaseGroup>> LoadPotentialMatchList(string url)
    {
        var currentPage = 1;
        var graphQLResponse = await startGGClient.SendQueryAsync<EventReponseType>(
            StartGGQueries.CreateEventSetsQuery(url, currentPage));
        var eventType = graphQLResponse.Data.Event;
        var nodes = eventType.Sets.Nodes;
        Dictionary<int, List<SetType>> phaseGroupLists = [];
        Dictionary<int, PhaseGroupType> phaseGroupTypes = [];
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

        var phaseGroups = phaseGroupTypes
            .OrderBy(pg => pg.Key) 
            .Select(pg => 
                new PhaseGroup(
                    pg.Value, 
                    phaseGroupLists[pg.Key]
                        .OrderBy(set => set.Identifier)
                        .ToList()
                    ))
            .ToList();

        return phaseGroups;
    }
}