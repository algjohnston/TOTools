using System.Collections.ObjectModel;
using TOTools.Database;
using TOTools.Models;

namespace TOTools.Seeding;

public class SeedingBusinessLogic(PlayerTable playerTable)
{
    public ObservableCollection<Player> GetAllPlayers()
    {
        return playerTable.SelectAll();   
    }

}