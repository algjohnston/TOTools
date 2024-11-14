﻿using System.Collections.ObjectModel;
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
    public ObservableCollection<Match> FutureMatches { get; } = [
        new Match("a", "b", 1, Game.Melee, false),
        new Match("b", "b", 1, Game.Melee, false),
        new Match("c", "b", 1, Game.Melee, false),
        new Match("d", "b", 1, Game.Melee, false)
    ];


    public long EstimateMatchLength(PastMatch match)
    {
        long totalTime = 0;
        int numMatches = 0;

        foreach (PastMatch pastMatch in PastMatches)
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

    private long GetAverageMatchLength(PastMatch match)
    {
        long totalTime = 0;
        int numMatches = 0;
        foreach (PastMatch pastMatch in PastMatches)
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


    private static bool AreMatchesComparable(PastMatch match1, PastMatch match2)
    {
        // matches are comparable if the games are the same, and they are either both best of 5, or best of 3
        return (match1.IsBestOfFive == match2.IsBestOfFive) && (match1.GameName.Equals(match2.GameName));
    }


    private bool ArePlayersEqual(Match match1, Match match2)
    {
        return (match1.Player1 == match2.Player1 && match1.Player2 == match2.Player2) ||
               (match2.Player1 == match2.Player2 && match1.Player2 == match1.Player1);
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