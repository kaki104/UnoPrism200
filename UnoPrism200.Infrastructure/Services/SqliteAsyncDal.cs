using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using UnoPrism200.Infrastructure.Interfaces;

namespace UnoPrism200.Infrastructure.Services
{
    public class SqliteAsyncDal : IDalAsync
    {
        private SQLiteAsyncConnection _db;

        private string DbPath { get; set; }

        /// <summary>
        /// SetDatabaseConnection
        /// </summary>
        /// <param name="databasePath">
        ///     Database storage path and file name
        /// </param>
        /// <returns>
        ///     true : Exists database file
        ///     false : Not exists database file
        /// </returns>
        public bool SetDatabaseConnection(string databasePath)
        {
            if (string.IsNullOrEmpty(databasePath))
            {
                throw new ArgumentNullException("databasePath cannot be null or empty. ex)c:\\MyData.db");
            }

            DbPath = databasePath;
            if (_db != null)
            {
                _db = new SQLiteAsyncConnection(DbPath);
            }

            return File.Exists(DbPath);
        }

        public async Task CreateTableAsync<T>() where T : class
        {
            if (_db != null)
            {
                _db = new SQLiteAsyncConnection(DbPath);
            }

            await _db.CreateTableAsync(typeof(T));
        }

        public async Task<int> InsertOrReplaceAsync<T>(T source) where T : class
        {
            try
            {
                int result = await _db.InsertOrReplaceAsync(source);
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return -1;
        }

        public async Task<int> InsertAsync<T>(T source) where T : class
        {
            int result = await _db.InsertAsync(source);
            return result;
        }

        public async Task<IList<T>> GetAllAsync<T>() where T : class, new()
        {
            //var db = new SQLiteAsyncConnection(DbPath);
            List<T> result = await _db.Table<T>().ToListAsync();
            return result;
        }

        public AsyncTableQuery<T> GetTable<T>() where T : class, new()
        {
            try
            {
                //var db = new SQLiteAsyncConnection(DbPath);
                return _db.Table<T>();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(object item)
        {
            //var db = new SQLiteAsyncConnection(DbPath);
            int result = await _db.UpdateAsync(item);
            return result;
        }

        public async Task<int> DeleteAsync(object deleteItem)
        {
            int result = await _db.DeleteAsync(deleteItem);
            return result;
        }
    }
}
