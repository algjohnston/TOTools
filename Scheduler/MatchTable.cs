using System.Collections.ObjectModel;
using CS341Project.Models;
using Npgsql;
using TOTools.Database;
using TOTools.Models;

namespace TOTools.Scheduler;

/// <summary>
/// Caden Rohan
/// The data source for Matches (usually called sets, but SetHistoryTable is a bit confusing)
/// </summary>
public class MatchTable : ITable<Match, long>
{
    private const string MatchTableName = "matches";
    private const string MatchIdColumn = "match_id";
    private const string Player1IdColumn = "player_1_id";
    private const string Player2IdColumn = "player_2_id";
    private const string MatchTimeColumn = "match_time";
    private const string GameNameColumn = "game_name";
    private const string IsBestOfFiveColumn = "is_best_of_five";
    
    private const int MatchIdColumnNumber = 0;
    private const int Player1IdColumnNumber = 1;
    private const int Player2IdColumnNumber = 2;
    private const int MatchTimeColumnNumber = 3;
    private const int GameNameColumnNumber = 4;
    private const int IsBestOfFiveColumnNumber = 5;

    private readonly ObservableCollection<Match> matches = [];

    public MatchTable()
    {
        const string createTableStatement =
            "CREATE TABLE " +
            "IF NOT EXISTS " +
            $"{MatchTableName} (" +
            $"{MatchIdColumn} BIGSERIAL PRIMARY KEY, " +
            $"{Player1IdColumn} TEXT, " +
            $"{Player2IdColumn} TEXT, " +
            $"{MatchTimeColumn} BIGINT, " +
            $"{GameNameColumn} INT " +
            $"{IsBestOfFiveColumn} BOOL " +
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
            $"{GameNameColumn} = @game_name, " +
            $"{IsBestOfFiveColumn} = @is_best_of_five, " +
            $"WHERE {MatchIdColumn} = @match_id;";
        command.Parameters.AddWithValue("match_id", toUpdate.MatchId);
        command.Parameters.AddWithValue("player_1_id", toUpdate.Player1);
        command.Parameters.AddWithValue("player_2_id", toUpdate.Player2);
        command.Parameters.AddWithValue("match_time", toUpdate.MatchTime);
        command.Parameters.AddWithValue("game_name", toUpdate.GameName);
        command.Parameters.AddWithValue("is_best_of_five", toUpdate.isBestOfFive);

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
            $"{MatchIdColumn}, {Player1IdColumn}, {Player2IdColumn}, {MatchTimeColumn}, {GameNameColumn}" +
            $") VALUES " +
            $"(@name, @location, @start_datetime, @end_datetime, @game, @is_best_of_five)";
        command.Parameters.AddWithValue("player_1_id", toInsert.Player1);
        command.Parameters.AddWithValue("player_2_id", toInsert.Player2);
        command.Parameters.AddWithValue("match_time", toInsert.MatchTime);
        command.Parameters.AddWithValue("game", toInsert.GameName);
        command.Parameters.AddWithValue("is_best_of_five", toInsert.isBestOfFive);
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
            $"SELECT {MatchIdColumn}, {Player1IdColumn}, {Player2IdColumn}, {MatchTimeColumn}, {GameNameColumn}, {IsBestOfFiveColumn} FROM {MatchTableName}",
            connection
        );
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var matchId = reader.GetInt64(MatchIdColumnNumber);
            var player1Id = reader.GetString(Player1IdColumnNumber);
            var player2Id = reader.GetString(Player2IdColumnNumber);
            var matchTime = reader.GetInt64(MatchTimeColumnNumber);
            var gameName = reader.GetInt16(GameNameColumnNumber);
            var isBestOfFive = reader.GetBoolean(IsBestOfFiveColumnNumber);
            Match matchToAdd = new(matchId, player1Id, player2Id, matchTime, GameHelper.ConvertToGame(gameName), isBestOfFive);
            matches.Add(matchToAdd);
        }

        return matches;
    }
}