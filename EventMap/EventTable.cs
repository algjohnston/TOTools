using System.Collections.ObjectModel;
using CS341Project.Database;
using CS341Project.Models;
using Npgsql;

namespace CS341Project.EventMap;

/// <summary>
/// Alexander Johnston
/// The data source for Events.
/// </summary>
public class EventTable : ITable<Event>
{
    private const string EventTableName = "events";
    private const string EventIdColumn = "event_id";
    private const string EventNameColumn = "name";
    private const string EventLocationColumn = "location";
    private const string EventStartDateTimeColumn = "start_date_time";
    private const string EventEndDateTimeColumn = "end_date_time";

    private readonly ObservableCollection<Event> events = [];

    public EventTable()
    {
        const string createTableStatement =
            "CREATE TABLE " +
            "IF NOT EXISTS " +
            $"{EventTableName} (" +
            $"{EventIdColumn} BIGSERIAL PRIMARY KEY, " +
            $"{EventNameColumn} TEXT, " +
            $"{EventLocationColumn} TEXT, " +
            $"{EventStartDateTimeColumn} TIMESTAMPTZ, " +
            $"{EventEndDateTimeColumn} TIMESTAMPTZ" +
            ")";
        DatabaseUtil.CreateTable(createTableStatement);
    }

    public void Delete(long id)
    {
        using var command = new NpgsqlCommand();
        command.Connection = DatabaseUtil.GetDatabaseConnection();
        command.CommandText = $"DELETE FROM {EventTableName} WHERE {EventIdColumn} = @id";
        command.Parameters.AddWithValue("id", id);
        var numDeleted = command.ExecuteNonQuery();
        if (numDeleted > 0)
        {
            SelectAll();
        }
    }

    public void Update(Event toUpdate)
    {
        using var command = new NpgsqlCommand();
        command.Connection = DatabaseUtil.GetDatabaseConnection();
        command.CommandText =
            $"UPDATE {EventTableName} " +
            $"SET {EventIdColumn} = @id, " +
            $"{EventNameColumn} = @name, " +
            $"{EventLocationColumn} = @location, " +
            $"{EventStartDateTimeColumn} = @start_datetime, " +
            $"{EventEndDateTimeColumn} = @end_datetime" +
            $"WHERE {EventIdColumn} = @id;";
        command.Parameters.AddWithValue("id", toUpdate.EventId);
        command.Parameters.AddWithValue("name", toUpdate.EventName);
        command.Parameters.AddWithValue("location", toUpdate.Location);
        command.Parameters.AddWithValue("start_datetime", toUpdate.StartDateTime);
        command.Parameters.AddWithValue("end_datetime", toUpdate.EndDateTime);

        var numAffected = command.ExecuteNonQuery();
        if (numAffected > 0)
        {
            SelectAll();
        }
    }

    public void Insert(Event toInsert)
    {
        using var command = new NpgsqlCommand();
        command.Connection = DatabaseUtil.GetDatabaseConnection();
        command.CommandText =
            $"INSERT INTO {EventTableName} (" +
            $"{EventIdColumn}, {EventNameColumn}, {EventLocationColumn}, {EventStartDateTimeColumn}, {EventEndDateTimeColumn}" +
            $") VALUES " +
            $"(@name, @location, @start_datetime, @end_datetime)";
        command.Parameters.AddWithValue("name", toInsert.EventName);
        command.Parameters.AddWithValue("location", toInsert.Location);
        command.Parameters.AddWithValue("start_datetime", toInsert.StartDateTime);
        command.Parameters.AddWithValue("end_datetime", toInsert.EndDateTime);
        command.ExecuteNonQuery();
        SelectAll();
    }

    public Event? Select(long id)
    {
        return events.SingleOrDefault(
            x => x?.EventId.Equals(id) ?? false,
            null
        );
    }

    public ObservableCollection<Event> SelectAll()
    {
        events.Clear();
        using var command = new NpgsqlCommand(
            $"SELECT {EventIdColumn}, {EventNameColumn}, {EventLocationColumn}, {EventStartDateTimeColumn}, {EventEndDateTimeColumn} FROM {EventTableName}",
            DatabaseUtil.GetDatabaseConnection()
        );
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt64(0);
            var name = reader.GetString(1);
            var location = reader.GetString(2);
            var startDateTime = reader.GetDateTime(3);
            var endDateTime = reader.GetDateTime(4);
            Event eventToAdd = new(id, name, location, startDateTime, endDateTime);
            events.Add(eventToAdd);
        }

        return events;
    }
    
}