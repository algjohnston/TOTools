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
    private const string TableName = "events";
    private const string IdColumn = "event_id";
    private const string NameColumn = "name";
    private const string LocationColumn = "location";
    private const string StartDateTimeColumn = "start_date_time";
    private const string EndDateTimeColumn = "end_date_time";
    
    private const int IdColumnNumber = 0;
    private const int NameColumnNumber = 1;
    private const int LocationColumnNumber = 2;
    private const int StartDateTimeColumnNumber = 3;
    private const int EndDateTimeColumnNumber = 4;

    private readonly ObservableCollection<Event> events = [];

    public EventTable()
    {
        const string createTableStatement =
            "CREATE TABLE " +
            "IF NOT EXISTS " +
            $"{TableName} (" +
            $"{IdColumn} BIGSERIAL PRIMARY KEY, " +
            $"{NameColumn} TEXT, " +
            $"{LocationColumn} TEXT, " +
            $"{StartDateTimeColumn} TIMESTAMPTZ, " +
            $"{EndDateTimeColumn} TIMESTAMPTZ" +
            ")";
        DatabaseUtil.CreateTable(createTableStatement);
    }

    public void Delete(long id)
    {
        using var command = new NpgsqlCommand();
        command.Connection = DatabaseUtil.GetDatabaseConnection();
        command.CommandText = $"DELETE FROM {TableName} WHERE {IdColumn} = @id";
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
            $"UPDATE {TableName} " +
            $"SET {IdColumn} = @id, " +
            $"{NameColumn} = @name, " +
            $"{LocationColumn} = @location, " +
            $"{StartDateTimeColumn} = @start_datetime, " +
            $"{EndDateTimeColumn} = @end_datetime" +
            $"WHERE {IdColumn} = @id;";
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
            $"INSERT INTO {TableName} (" +
            $"{IdColumn}, {NameColumn}, {LocationColumn}, {StartDateTimeColumn}, {EndDateTimeColumn}" +
            $") VALUES " +
            $"(@id, @name, @location, @start_datetime, @end_datetime)";
        command.Parameters.AddWithValue("id", toInsert.EventId);
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
            $"SELECT {IdColumn}, {NameColumn}, {LocationColumn}, {StartDateTimeColumn}, {EndDateTimeColumn} FROM {TableName}",
            DatabaseUtil.GetDatabaseConnection()
        );
        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            var id = reader.GetInt64(IdColumnNumber);
            var name = reader.GetString(NameColumnNumber);
            var location = reader.GetString(LocationColumnNumber);
            var startDateTime = reader.GetDateTime(StartDateTimeColumnNumber);
            var endDateTime = reader.GetDateTime(EndDateTimeColumnNumber);
            Event eventToAdd = new(id, name, location, startDateTime, endDateTime);
            events.Add(eventToAdd);
        }

        return events;
    }
    
}