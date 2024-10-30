using Npgsql;

namespace CS341Project.Database;

/// <summary>
/// Alexander Johnston
/// A collection of static methods for working with databases.
/// </summary>
public static class DatabaseUtil
{
    /// <summary>
    /// Creates the given table in the database.
    /// </summary>
    /// <param name="tableCommandString">The string with the create table command</param>
    public static void CreateTable(string tableCommandString)
    {
        using var connection = GetDatabaseConnection();
        new NpgsqlCommand(
            tableCommandString,
            connection
        ).ExecuteNonQuery();
    }

    /// <summary>
    /// Gets the string used to connect to the database.
    /// </summary>
    /// <param name="databaseName">The name of the database to be connected to.</param>
    /// <returns>The string for connecting to the database.</returns>
    private static string GetConnectionString(string databaseName)
    {
        var connStringBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = "sulky-quagga-2108.jxf.gcp-us-central1.cockroachlabs.cloud",
            Port = 26257,
            SslMode = SslMode.VerifyFull,
            Username = "main",
            Password = "P0TNUJ1YdhaTJ6B6Mxx-lg",
            Database = databaseName,
            ApplicationName = "CS341Project",
            IncludeErrorDetail = true
        };
        return connStringBuilder.ConnectionString;
    }

    /// <summary>
    /// Opens the database and returns a connection to the database.
    /// </summary>
    /// <returns>A connection to the database</returns>
    public static NpgsqlConnection GetDatabaseConnection()
    {
        const string databaseName = "test";
        // CreateDatabaseIfItDoesNotExist(databaseName); // Is not needed if the database is created in the cockroachdb console
        var databaseConnection = new NpgsqlConnection(GetConnectionString(databaseName));
        databaseConnection.Open();
        return databaseConnection;
    }

    /// <summary>
    /// Creates a database if it does not exist.
    /// </summary>
    /// <param name="databaseName">The name of the database to check for.</param>
    private static void CreateDatabaseIfItDoesNotExist(string databaseName)
    {
        // postgres is a default database for applications to connect to
        using var postgresDatabaseConnection = new NpgsqlConnection(
            GetConnectionString("postgres")
        );
        postgresDatabaseConnection.Open();
        
        // pg_database stores information about the available databases
        // pg_database is shared across all databases of a cluster
        using var command = new NpgsqlCommand(
            $"SELECT 1 FROM pg_database WHERE datname = '{databaseName}'",
            postgresDatabaseConnection
        );
        if (command.ExecuteScalar() == null)
        {
            using var createCommand = new NpgsqlCommand(
                $"CREATE DATABASE \"{databaseName}\"",
                postgresDatabaseConnection
            );
            createCommand.ExecuteNonQuery();
        }
    }

}