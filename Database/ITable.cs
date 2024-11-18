using System.Collections.ObjectModel;

namespace TOTools.Database;

/// <summary>
/// Alexander Johnston
/// An interface for databases.
/// </summary>
/// <typeparam name="T">The type to be stored in this database.</typeparam>
/// <typeparam name="TIdType">The type of the id column.</typeparam>
/// <typeparam name="TInsertType">The type that is inserted into the database</typeparam>
public interface ITable<T, in TIdType, in TInsertType>
{
    /// <summary>
    /// Deletes the record from this database with the given id.
    /// </summary>
    /// <param name="id">The id of the record to delete.</param>
    void Delete(TIdType id);
    
    /// <summary>
    /// Updates the record with the same id as toUpdate.
    /// </summary>
    /// <param name="toUpdate">The updated record.</param>
    void Update(T toUpdate);
    
    /// <summary>
    /// Inserts toInsert into the database.
    /// </summary>
    /// <param name="toInsert">The record to insert.</param>
    void Insert(TInsertType toInsert);
    
    /// <summary>
    /// Selects the record with the given id.
    /// </summary>
    /// <param name="id">The id of the record to select.</param>
    /// <returns>The record with the given id if it exists, else null.</returns>
    T? Select(TIdType id);
    
    /// <summary>
    /// Selects all records form the database.
    /// </summary>
    /// <returns>All records from the database</returns>
    ObservableCollection<T> SelectAll();
    
}