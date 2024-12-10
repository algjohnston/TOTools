using System.Collections.ObjectModel;
using TOTools.Database;
using TOTools.Models;
using TOTools.Models.Startgg;
using TOTools.Seeding;

namespace TOTools.Scheduler;

/// <summary>
/// Business logic for the scheduler.
/// </summary>
public class SchedulerBusinessLogic(
    SeedingBusinessLogic seedingBusinessLogic,
    MatchTable matchTable)
{
    public ObservableCollection<EventLink?> EventLinks { get; } =
    [
        // For testing
        new(
            "tournament/la-mechita-2024-torneo-feria-esports-zona-retro-y-m/event/ultimate-singles",
            DateTime.Now,
            3)
    ];

    // Has all matches played previously by any players in any game; used to estimate time
    private ObservableCollection<PastMatch> PastMatches { get; } = [];

    // This gets filled with matches from startgg that need to be played
    public ObservableCollection<EventMatchGroup> FutureMatches { get; } = [];
    
    private Dictionary<string, EventBracketGroup> EventBrackets { get; } = [];

    public EventLink? SelectedEventLink { get; set; } // For editing the event link

    public Match? SelectedMatch { get; set; } // For editing matches

    private readonly TaskCompletionSource<bool> _loadCompletionSource = new();
    public Task PastMatchLoadTask => _loadCompletionSource.Task;

    /// <summary>
    /// Loads the past matches from the database.
    /// 
    /// </summary>
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

    /// <summary>
    /// Adds an event link to be used for the schedule generation.
    /// </summary>
    /// <param name="eventLink">A startgg event link.</param>
    public void AddEventLink(EventLink eventLink)
    {
        if (!EventLinks.Contains(eventLink))
        {
            EventLinks.Add(eventLink);
        }
    }

    /// <summary>
    /// Removes an event link from the schedule generator
    /// </summary>
    /// <param name="eventLink">The link to remove.</param>
    public void RemoveEvent(EventLink eventLink)
    {
        EventLinks.Remove(eventLink);
    }

    /// <summary>
    /// Estimates how long a match between two player will be.
    /// </summary>
    /// <param name="player1">The first player.</param>
    /// <param name="player2">The second player.</param>
    /// <returns>The estimated time, in seconds, of the match.</returns>
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

    /// <summary>
    /// Gets the average length of all past matches.
    /// </summary>
    /// <returns>The average length, in seconds, of all past matches.</returns>
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

        // TODO had to change to 0
        // this will return -1 if there is nothing in the table for previous matches, but this should basically never happen
        return (numMatches != 0) ? totalTime / numMatches : 0;
    }

    /// <summary>
    /// Checks if two matches have the same game and number of plays.
    /// </summary>
    /// <param name="match1">One match.</param>
    /// <param name="match2">Another match.</param>
    /// <returns>True if the matches are the same game with the same number of plays, else false.</returns>
    private static bool AreMatchesComparable(Match match1, PastMatch match2)
    {
        // matches are comparable if the games are the same, and they are either both best of 5, or best of 3
        return (match1.IsBestOfFive == match2.IsBestOfFive) && (match1.GameName.Equals(match2.GameName));
    }

    /// <summary>
    /// Checks if the players are the same as the ones in the given match.
    /// </summary>
    /// <param name="player1">One player.</param>
    /// <param name="player2">The other player.</param>
    /// <param name="match">The match to check the layers of.</param>
    /// <returns>True if the players in the match are the same as player1 and player2, else false.</returns>
    private static bool ArePlayersEqual(string player1, string player2, Match match)
    {
        return (player1 == match.Player1 && player2 == match.Player2) ||
               (match.Player1 == match.Player2 && player2 == player1);
    }

    /// <summary>
    /// Generates a match schedule for all phase groups in an all events whose links were added via AddEventLink.
    /// </summary>
    /// <param name="eventName"></param>
    /// <param name="eventStartTime"></param>
    /// <param name="phaseGroups">Every phase group of all the events.</param>
    private void GenerateMatchSchedule(string eventName, DateTime eventStartTime, List<PhaseGroup> phaseGroups)
    {
        // Prevents duplicates from being added
        if (FutureMatches.FirstOrDefault(x => x.EventName == eventName) != null)
        {
            return;
        }

        // Only get the sets that have two players...
        var matchParticipants = phaseGroups.SelectMany(
            phaseGroup => phaseGroup.Sets.Where(
                set => set.Slots is [{ Entrant: not null }, { Entrant: not null }])
        ).ToDictionary(set => set, set => EstimateMatchLength(set.Slots[0].Entrant!.Name, set.Slots[1].Entrant!.Name));

        // ... and sort by the estimated match length
        var sortedMatchParticipants = matchParticipants
            .OrderByDescending(kv => kv.Value)
            .ToList();

        // TODO get game and isBestOfFive
        // Create the matches for the UI
        var futureMatches = sortedMatchParticipants
            .Select(kvp => new Match(
                kvp.Key.Id,
                kvp.Key.Slots[0].Entrant!.Name,
                kvp.Key.Slots[1].Entrant!.Name,
                kvp.Value,
                Game.Unknown,
                true
                )
            );
        FutureMatches.Add(new EventMatchGroup(eventName, eventStartTime, futureMatches));
    }

    /// <summary>
    /// Loads a potential schedule of all the matches into FutureMatches in the recommended match ordering.
    /// </summary>
    public async Task LoadPotentialSchedule()
    {
        // Need to make sure that the past matches are loaded for the time estimation
        await seedingBusinessLogic.PlayerLoadTask;

        // Generate the schedule and add the brackets of each event to the seeding business logic
        seedingBusinessLogic.ClearBrackets();
        foreach (var eventLink in EventLinks.OrderBy(e => e?.StartTime))
        {
            if (eventLink == null)
            {
                continue;
            }

            var phaseGroups = await seedingBusinessLogic.LoadPhaseGroups(eventLink.Link);
            var eventName = EventLink.ExtractEventName(eventLink.Link);
            GenerateMatchSchedule(eventName, eventLink.StartTime, phaseGroups);
            var bracket = new EventBracketGroup(eventName, phaseGroups);
            EventBrackets.Add(eventName, bracket);
        }
    }

    /// <summary>
    /// Swaps two event groups in the list of match groups.
    /// </summary>
    /// <param name="sourceGroup"></param>
    /// <param name="destinationPlayerGroup"></param>
    public void SwapEventGroups(
        EventMatchGroup sourceGroup,
        EventMatchGroup destinationPlayerGroup
    )
    {
        var sourceIndex = FutureMatches.IndexOf(sourceGroup);
        var destinationIndex = FutureMatches.IndexOf(destinationPlayerGroup);
        if (sourceIndex == destinationIndex)
        {
            return;
        }

        FutureMatches.Move(sourceIndex, destinationIndex);
    }

    public void ReportMatch(string eventName, Match match, string winner)
    {
        var id = match.SetId;
        if (id == null)
        {
            return;
        }
        EventBrackets[eventName].RecordWinner(winner, id);
    }
}