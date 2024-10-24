using System.Collections.ObjectModel;
using CS341Project.Database;
using CS341Project.Models;
using Npgsql;

namespace CS341Project.Seeding;

/// <summary>
/// Caden Rohan
/// The data source for Players
/// </summary>
public class PlayerTable : IDatabaseTable<Player>
{
    private const string PlayerTableName = "players";
    private const string PlayerIdColumn = "id";
    private const string PlayerTagColumn = "tag";
    private const string PlayerRegionColumn = "region";
    private const string PlayerTierColumn = "tier";
    private const string PlayerRankingColumn = "ranking"; // needs a better name

    private readonly ObservableCollection<Player> players = [];
    
    public PlayerTable()
    {
        const string createTableStatement =
            "CREATE TABLE " +
            "IF NOT EXISTS " +
            $"{PlayerTableName} (" +
            $"{PlayerIdColumn} BIGSERIAL PRIMARY KEY, " +
            $"{PlayerTagColumn} TEXT, " +
            $"{PlayerRegionColumn} TEXT, " +
            $"{PlayerTierColumn} TEXT, " +
            $"{PlayerRankingColumn} INT" +
            ")";
        DatabaseUtil.CreateTable(createTableStatement);
    }
    
    public void Delete(long id)
    {
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = $"DELETE FROM {PlayerTableName} WHERE {PlayerIdColumn} = @id";
        command.Parameters.AddWithValue("id", id);
        var numDeleted = command.ExecuteNonQuery();
        if (numDeleted > 0)
        {
            SelectAll();
        }
    }
    
    public void Update(Player toUpdate)
    {
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText =
            $"UPDATE {PlayerTableName} " +
            $"SET {PlayerIdColumn} = @id, " +
            $"{PlayerTagColumn} = @tag, " +
            $"{PlayerRegionColumn} = @region, " +
            $"{PlayerTierColumn} = @tier, " +
            $"{PlayerRankingColumn} = @ranking" +
            $"WHERE {PlayerIdColumn} = @id;";
        command.Parameters.AddWithValue("id", toUpdate.PlayerId);
        command.Parameters.AddWithValue("tag", toUpdate.PlayerTag);
        command.Parameters.AddWithValue("region", toUpdate.PlayerRegion);
        command.Parameters.AddWithValue("tier", toUpdate.PlayerTier);
        command.Parameters.AddWithValue("ranking", toUpdate.PlayerRanking);

        var numAffected = command.ExecuteNonQuery();
        if (numAffected > 0)
        {
            SelectAll();
        }
    }
    
    public void Insert(Player toInsert)
    {
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText =
            $"INSERT INTO {PlayerTableName} (" +
            $"{PlayerIdColumn}, {PlayerTagColumn}, {PlayerRegionColumn}, {PlayerTierColumn}, {PlayerRankingColumn}" +
            $") VALUES " +
            $"(@tag, @region, @tier, @ranking)";
        command.Parameters.AddWithValue("tag", toInsert.PlayerTag);
        command.Parameters.AddWithValue("region", toInsert.PlayerRegion);
        command.Parameters.AddWithValue("tier", toInsert.PlayerTier);
        command.Parameters.AddWithValue("ranking", toInsert.PlayerRanking);
        command.ExecuteNonQuery();
        SelectAll();
    }
    
    public Player? Select(long id)
    {
        return players.SingleOrDefault(
            x => x?.PlayerId.Equals(id) ?? false,
            null
        );
    }
    
    public ObservableCollection<Player> SelectAll()
    {
        players.Clear();
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand(
            $"SELECT {PlayerIdColumn}, {PlayerTagColumn}, {PlayerRegionColumn}, {PlayerTierColumn}, {PlayerRankingColumn} FROM {PlayerTableName}",
            connection
        );
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt64(0);
            var tag = reader.GetString(1);
            var region = reader.GetString(2);
            var tier = reader.GetString(3);
            var ranking = reader.GetInt32(4);
            Player playerToAdd = new(id, tag, region, TierHelper.ConvertToTier(tier), ranking);
            players.Add(playerToAdd);
        }

        return players;
    }
    
    
}