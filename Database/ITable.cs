using System.Collections.ObjectModel;

namespace CS341Project.Database;

/// <summary>
/// Alexander Johnston
/// An interface for databases.
/// </summary>
/// <typeparam name="T">The type to be stored in this database.</typeparam>
public interface ITable<T>
{
    /// <summary>
    /// Deletes the record from this database with the given id.
    /// </summary>
    /// <param name="id">The id of the record to delete.</param>
    void Delete(long id);
    
    /// <summary>
    /// Updates the record with the same id as toUpdate.
    /// </summary>
    /// <param name="toUpdate">The updated record.</param>
    void Update(T toUpdate);
    
    /// <summary>
    /// Inserts toInsert into the database.
    /// </summary>
    /// <param name="toInsert">The record to insert.</param>
    void Insert(T toInsert);
    
    /// <summary>
    /// Selects the record with the given id.
    /// </summary>
    /// <param name="id">The id of the record to select.</param>
    /// <returns>The record with the given id if it exists, else null.</returns>
    T? Select(long id);
    
    /// <summary>
    /// Selects all records form the database.
    /// </summary>
    /// <returns>All records from the database</returns>
    ObservableCollection<T> SelectAll();
    
}