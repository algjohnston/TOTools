using System.Collections.ObjectModel;
using Npgsql;
using TOTools.Models;

namespace TOTools.Database;

/// <summary>
/// Caden Rohan
/// The data source for Matches (usually called sets, but SetHistoryTable is a bit confusing)
/// </summary>
public class MatchTable : ITable<PastMatch, long, Match>
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

    private readonly ObservableCollection<PastMatch> _matches = [];

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
            $"{GameNameColumn} INT, " +
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

    public void Update(PastMatch match)
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
        command.Parameters.AddWithValue("match_id", match.MatchId);
        command.Parameters.AddWithValue("player_1_id", match.Player1Id);
        command.Parameters.AddWithValue("player_2_id", match.Player2Id);
        command.Parameters.AddWithValue("match_time", match.MatchTime);
        command.Parameters.AddWithValue("game_name", (int)match.GameName);
        command.Parameters.AddWithValue("is_best_of_five", match.IsBestOfFive);

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
            $"{Player1IdColumn}, {Player2IdColumn}, {MatchTimeColumn}, {GameNameColumn}, {IsBestOfFiveColumn}" +
            $") VALUES " +
            $"(@player_1_id, @player_2_id, @match_time, @game, @is_best_of_five)";
        command.Parameters.AddWithValue("player_1_id", toInsert.Player1Id);
        command.Parameters.AddWithValue("player_2_id", toInsert.Player2Id);
        command.Parameters.AddWithValue("match_time", toInsert.TimeInSeconds);
        command.Parameters.AddWithValue("game", (int)toInsert.GameName);
        command.Parameters.AddWithValue("is_best_of_five", toInsert.IsBestOfFive);
        command.ExecuteNonQuery();
        SelectAll();
    }

    public PastMatch? Select(long id)
    {
        return _matches.SingleOrDefault(
            x => x?.MatchId.Equals(id) ?? false,
            null
        );
    }

    public ObservableCollection<PastMatch> SelectAll()
    {
        _matches.Clear();
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
            PastMatch matchToAdd = new(matchId, player1Id, player2Id, matchTime, GameHelper.ConvertToGame(gameName),
                isBestOfFive);
            _matches.Add(matchToAdd);
        }

        return _matches;
    }
}