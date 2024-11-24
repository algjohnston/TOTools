using System.Collections.ObjectModel;
using CS341Project.Models;
using TOTools.Database;
using TOTools.Models;
using TOTools.Models.Startgg;
using TOTools.Seeding;

namespace TOTools.Scheduler;

/// <summary>
/// Business logic for the scheduler
/// </summary>
public class SchedulerBusinessLogic(
    SeedingBusinessLogic seedingBusinessLogic,
    MatchTable matchTable)
{
    public ObservableCollection<EventLink> EventLinks { get; } =
    [
        // For testing
        new(
            "tournament/between-2-lakes-67-a-madison-super-smash-bros-tournament/event/ultimate-singles",
            DateTime.Now,
            3)
    ];

    private ObservableCollection<PastMatch> PastMatches { get; } = [];

    // This gets filled with matches from startgg that need to be played
    public ObservableCollection<Match> FutureMatches { get; } = [];
    
    // has all matches played previously by any players in any game, used to estimate time

    private readonly TaskCompletionSource<bool> _loadCompletionSource = new();
    public Task LoadTask => _loadCompletionSource.Task;
    
    public void LoadPastMatches()
    {
        var matches = matchTable.SelectAll();
        PastMatches.Clear();
        foreach (var match in matches)
        {
            PastMatches.Add(match);
        }
        _loadCompletionSource.TrySetResult(true);
    }

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

    private void GenerateMatchSchedule(List<PhaseGroup> phaseGroups)
    {
        var matchParticipants = phaseGroups.SelectMany(
            phaseGroup => phaseGroup.Sets.Where(
                set => set.Slots.Count == 2)
        ).ToDictionary(set => set, set => EstimateMatchLength(set.Slots[0].Entrant.Name, set.Slots[1].Entrant.Name));

        var sortedMatchParticipants = matchParticipants
            .OrderByDescending(kv => kv.Value)
            .ToList();

        // TODO get game and isBestOfFive
        var futureMatches = sortedMatchParticipants
            .Select(kvp => new Match(
                kvp.Key.Slots[0].Entrant.Name,
                kvp.Key.Slots[1].Entrant.Name,
                kvp.Value,
                Game.Unknown,
                true));
        foreach (var match in futureMatches)
        {
            FutureMatches.Add(match);
        }
    }

    public async Task LoadPotentialSchedule()
    {
        await seedingBusinessLogic.LoadTask;
        foreach (var eventLink in EventLinks.OrderBy(e => e.StartTime))
        {
            var phaseGroups = await seedingBusinessLogic.LoadPhaseGroups(eventLink.Link);
            GenerateMatchSchedule(phaseGroups);
            var bracket = new Brackets(phaseGroups);
            seedingBusinessLogic.SetBracket(bracket);
        }
    }
}