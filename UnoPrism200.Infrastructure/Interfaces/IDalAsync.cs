using SQLite;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UnoPrism200.Infrastructure.Interfaces
{
    public interface IDalAsync
    {
        bool SetDatabaseConnection(string databasePath);

        Task<IList<T>> GetAllAsync<T>() where T : class, new();

        Task CreateTableAsync<T>() where T : class;

        Task<int> InsertOrReplaceAsync<T>(T source) where T : class;

        Task<int> InsertAsync<T>(T source) where T : class;

        AsyncTableQuery<T> GetTable<T>() where T : class, new();

        Task<int> UpdateAsync(object item);

        Task<int> DeleteAsync(object deleteItem);
    }
}