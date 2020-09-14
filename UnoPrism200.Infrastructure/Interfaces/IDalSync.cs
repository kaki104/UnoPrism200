using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnoPrism200.Infrastructure.Interfaces
{
    public interface IDalSync
    {
        bool SetDatabaseConnection(string databasePath);

        IList<T> GetAll<T>() where T : class, new();

        CreateTableResult CreateTable<T>() where T : class;

        int InsertOrReplace<T>(T source) where T : class;

        int Insert<T>(T source) where T : class;

        TableQuery<T> GetTable<T>() where T : class, new();

        int Update(object item);

        int Delete(object deleteItem);

    }
}
