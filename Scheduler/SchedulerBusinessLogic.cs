using System.Collections.ObjectModel;
using GraphQL.Client.Http;
using TOTools.Database;
using TOTools.Models;
using TOTools.Seeding;

namespace TOTools.Scheduler;

/// <summary>
/// Business logic for the scheduler
/// </summary>
public class SchedulerBusinessLogic(GraphQLHttpClient client)
{
    
    private readonly PlayerTable _playerTable = PlayerTable.GetPlayerTable();
    private readonly ITable<PastMatch, long> _table = new MatchTable();
        
    // this has all matches played previously by any players in any game, used to estimate time
    public ObservableCollection<PastMatch> PastMatches => _table.SelectAll();
    // This gets filled with matches from startgg that need to be played
    public ObservableCollection<Match> CurrentMatches { get; }  = [];

    // public async Task LoadPotentialMatchList(string url)
    // {
    //     var numberOfEntrantsResult = await client.GetNumberOfEntrants.ExecuteAsync(url);
    //     var numberOfEntrantsResultData = numberOfEntrantsResult.Data;
    //     if (numberOfEntrantsResultData?.Event?.NumEntrants == null)
    //     {
    //         throw new Exception("No entrants found"); // TODO remove
    //         return;
    //     }
    //     var numberOfEntrants = numberOfEntrantsResultData.Event.NumEntrants;
    //     
    //     var currentPage = 1;
    //     var entrantsResultData = await GetEntrantResultData(url, numberOfEntrants, currentPage);
    //     if (entrantsResultData?.Event?.Entrants?.PageInfo == null)
    //     {
    //         throw new Exception("No entrants found"); // TODO remove
    //         return;
    //     }
    //     var players = GetPlayers(entrantsResultData);
    //     
    //     var pageInfo = entrantsResultData.Event.Entrants.PageInfo;
    //     if (pageInfo.PerPage < numberOfEntrants)
    //     {
    //         while (pageInfo.Page < pageInfo.TotalPages)
    //         {
    //             currentPage++;
    //             entrantsResultData = await GetEntrantResultData(url, numberOfEntrants, currentPage);
    //             if (entrantsResultData?.Event?.Entrants?.PageInfo == null)
    //             {
    //                 throw new Exception("No entrants found"); // TODO remove
    //                 return;
    //             }
    //             players.AddRange(GetPlayers(entrantsResultData));
    //             pageInfo = entrantsResultData.Event.Entrants.PageInfo;
    //         }
    //     }
    //     
    //     for (int i = 0; i < players.Count; i += 2)
    //     {
    //         CurrentMatches.Add(new Match(players[i].PlayerTag, players[i + 1].PlayerTag));
    //     }
    //     
    // }
    //
    // private async Task<StartGG.GraphQL.IGetEntrantsResult?> GetEntrantResultData(string url, int? numberOfEntrants, int currentPage)
    // {
    //     var entrantsResult = await client.GetEntrants.ExecuteAsync(url, numberOfEntrants, currentPage);
    //     var entrantsResultData = entrantsResult.Data;
    //     if (entrantsResultData?.Event?.Entrants?.PageInfo == null ||
    //         entrantsResultData?.Event?.Entrants?.Nodes == null)
    //     {
    //         return entrantsResultData;
    //     }
    //
    //     return entrantsResultData;
    // }
    //
    // private List<Player> GetPlayers(StartGG.GraphQL.IGetEntrantsResult entrantsResultData)
    // {
    //     List<Player> players = [];
    //     if (entrantsResultData?.Event?.Entrants?.Nodes == null)
    //     {
    //         return players;
    //     }
    //     var nodes = entrantsResultData.Event.Entrants.Nodes;
    //     foreach (var node in nodes)
    //     {
    //         if (node?.Id == null)
    //         {
    //             continue;
    //         }
    //         var player = _playerTable.Select(node.Id);
    //         if (player != null)
    //         {
    //             players.Add(player);
    //         }
    //     }
    //     return players;
    // }

    public object LoadPotentialMatchList(string url)
    {
        throw new NotImplementedException();
    }
}