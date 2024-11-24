using System.Collections.ObjectModel;
using Npgsql;
using TOTools.Models;

namespace TOTools.Database;

/// <summary>
/// Caden Rohan
/// The data source for Players.
/// </summary>
public class PlayerTable : ITable<Player, string, Player>
{
    private const string PlayerTableName = "players";
    private const string StartggIdColumn = "startgg_id";
    private const string PlayerTagColumn = "tag";
    private const string PlayerRegionColumn = "region";
    private const string PlayerTierColumn = "tier";
    private const string PlayerRankingColumn = "ranking"; // needs a better name

    private const int StartggIdColumnNumber = 0;
    private const int PlayerTagColumnNumber = 1;
    private const int PlayerRegionColumnNumber = 2;
    private const int PlayerTierColumnNumber = 3;
    private const int PlayerRankingColumnNumber = 4; // needs a better name

    private readonly ObservableCollection<Player> _players = [];

    public PlayerTable()
    {
        const string createTableStatement =
            "CREATE TABLE " +
            "IF NOT EXISTS " +
            $"{PlayerTableName} (" +
            $"{StartggIdColumn} TEXT PRIMARY KEY, " +
            $"{PlayerTagColumn} TEXT, " +
            $"{PlayerRegionColumn} INT, " + // TODO Maybe an int(enum)?
            $"{PlayerTierColumn} INT, " + // TODO Maybe an int(enum)?
            $"{PlayerRankingColumn} INT" +
            ")";
        DatabaseUtil.CreateTable(createTableStatement);
    }

    public void Delete(string id)
    {
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = $"DELETE FROM {PlayerTableName} WHERE {StartggIdColumn} = @id";
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
            $"SET {StartggIdColumn} = @startgg_id" +
            $"{PlayerTagColumn} = @tag, " +
            $"{PlayerRegionColumn} = @region, " +
            $"{PlayerTierColumn} = @tier, " +
            $"{PlayerRankingColumn} = @ranking" +
            $"WHERE {StartggIdColumn} = @startgg_id;";
        command.Parameters.AddWithValue("startgg_id", toUpdate.StarttggId);
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
            $"{StartggIdColumn}, {PlayerTagColumn}, {PlayerRegionColumn}, {PlayerTierColumn}, {PlayerRankingColumn}" +
            $") VALUES " +
            $"(@start_gg_id, @tag, @region, @tier, @ranking)";
        command.Parameters.AddWithValue("start_gg_id", toInsert.StarttggId);
        command.Parameters.AddWithValue("tag", toInsert.PlayerTag);
        command.Parameters.AddWithValue("region", toInsert.PlayerRegion);
        command.Parameters.AddWithValue("tier", toInsert.PlayerTier);
        command.Parameters.AddWithValue("ranking", toInsert.PlayerRanking);
        command.ExecuteNonQuery();
        SelectAll();
    }

    public Player? Select(string id)
    {
        return _players.SingleOrDefault(
            x => x?.StarttggId.Equals(id) ?? false,
            null
        );
    }

    public ObservableCollection<Player> SelectAll()
    {
        _players.Clear();
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand(
            $"SELECT {StartggIdColumn}, {PlayerTagColumn}, {PlayerRegionColumn}, {PlayerTierColumn}, {PlayerRankingColumn} FROM {PlayerTableName}",
            connection
        );
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var startggId = reader.GetString(StartggIdColumnNumber);
            var tag = reader.GetString(PlayerTagColumnNumber);
            var region = reader.GetInt16(PlayerRegionColumnNumber);
            var tier = reader.GetInt16(PlayerTierColumnNumber);
            var ranking = reader.GetInt32(PlayerRankingColumnNumber);
            Player playerToAdd = new(
                startggId,
                tag,
                RegionHelper.ConvertToRegion(region),
                TierHelper.ConvertToTier(tier),
                ranking
            );
            _players.Add(playerToAdd);
        }

        return _players;
    }
}