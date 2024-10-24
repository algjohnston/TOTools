using System.Collections.ObjectModel;
using CS341Project.Database;
using CS341Project.Models;
using Npgsql;

namespace CS341Project.Scheduler;

/// <summary>
/// Caden Rohan
/// The data source for Matches (usually called sets, but SetHistoryTable is a bit confusing)
/// </summary>
public class MatchTable : IDatabaseTable<Match>
{
    private const string MatchTableName = "matches";
    private const string MatchIdColumn = "match_id";
    private const string Player1IdColumn = "player_1_id";
    private const string Player2IdColumn = "player_2_id";
    private const string MatchTimeColumn = "match_time";

    private readonly ObservableCollection<Match> matches = [];

    public MatchTable()
    {
        const string createTableStatement =
            "CREATE TABLE " +
            "IF NOT EXISTS " +
            $"{MatchTableName} (" +
            $"{MatchIdColumn} BIGSERIAL PRIMARY KEY, " +
            $"{Player1IdColumn} BIGSERIAL " +
            $"{Player2IdColumn} BIGSERIAL " +
            $"{MatchTimeColumn} BIGINT, " +
            ")";
        DatabaseUtil.CreateTable(createTableStatement);
    }

    public void Delete(long id)
    {
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText = $"DELETE FROM {MatchTableName} WHERE {MatchIdColumn} = @id";
        command.Parameters.AddWithValue("match_id", id);
        var numDeleted = command.ExecuteNonQuery();
        if (numDeleted > 0)
        {
            SelectAll();
        }
    }

    public void Update(Match toUpdate)
    {
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText =
            $"UPDATE {MatchTableName} " +
            $"SET {MatchIdColumn} = @match_id, " +
            $"{Player1IdColumn} = @player_1_id, " +
            $"{Player2IdColumn} = @player_2_id, " +
            $"{MatchTimeColumn} = @match_time, " +
            $"WHERE {MatchIdColumn} = @match_id;";
        command.Parameters.AddWithValue("match_id", toUpdate.MatchId);
        command.Parameters.AddWithValue("player_1_id", toUpdate.Player1);
        command.Parameters.AddWithValue("player_2_id", toUpdate.Player2);
        command.Parameters.AddWithValue("match_time", toUpdate.MatchTime);

        var numAffected = command.ExecuteNonQuery();
        if (numAffected > 0)
        {
            SelectAll();
        }
    }

    public void Insert(Match toInsert)
    {
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand();
        command.Connection = connection;
        command.CommandText =
            $"INSERT INTO {MatchTableName} (" +
            $"{MatchIdColumn}, {Player1IdColumn}, {Player2IdColumn}, {MatchTimeColumn}" +
            $") VALUES " +
            $"(@name, @location, @start_datetime, @end_datetime)";
        command.Parameters.AddWithValue("player_1_id", toInsert.Player1);
        command.Parameters.AddWithValue("player_2_id", toInsert.Player2);
        command.Parameters.AddWithValue("match_time", toInsert.MatchTime);
        command.ExecuteNonQuery();
        SelectAll();
    }

    public Match? Select(long id)
    {
        return matches.SingleOrDefault(
            x => x?.MatchId.Equals(id) ?? false,
            null
        );
    }

    public ObservableCollection<Match> SelectAll()
    {
        matches.Clear();
        using var connection = DatabaseUtil.GetDatabaseConnection();
        using var command = new NpgsqlCommand(
            $"SELECT {MatchIdColumn}, {Player1IdColumn}, {Player2IdColumn}, {MatchTimeColumn} FROM {MatchTableName}",
            connection
        );
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var match_id = reader.GetInt64(0);
            var player_1_id = reader.GetInt64(1);
            var player_2_id = reader.GetInt64(2);
            var match_time = reader.GetInt64(3);
            Match matchToAdd = new(match_id, player_1_id, player_2_id, match_time);
            matches.Add(matchToAdd);
        }

        return matches;
    }
}