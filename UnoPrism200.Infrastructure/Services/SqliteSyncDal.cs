using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using UnoPrism200.Infrastructure.Interfaces;

namespace UnoPrism200.Infrastructure.Services
{
    public class SqliteSyncDal : IDalSync
    {
        private SQLiteConnection _db;

        private string DbPath { get; set; }

        public CreateTableResult CreateTable<T>() where T : class
        {
            CheckDbConnection();
            return _db.CreateTable(typeof(T));
        }

        private void CheckDbConnection()
        {
            if (string.IsNullOrEmpty(DbPath))
            {
                throw new ArgumentNullException("DbPath cannot be null or empty. ex)c:\\MyData.db");
            }
            if(_db == null)
            {
                _db = new SQLiteConnection(DbPath);
            }
        }

        public int Delete(object deleteItem)
        {
            CheckDbConnection();
            return _db.Delete(deleteItem);
        }

        public IList<T> GetAll<T>() where T : class, new()
        {
            CheckDbConnection();
            return _db.Table<T>().ToList();
        }

        public TableQuery<T> GetTable<T>() where T : class, new()
        {
            try
            {
                CheckDbConnection();
                return _db.Table<T>();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return null;
        }

        public int Insert<T>(T source) where T : class
        {
            try
            {
                CheckDbConnection();
                return _db.Insert(source);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return -1;
        }

        public int InsertOrReplace<T>(T source) where T : class
        {
            try
            {
                CheckDbConnection();
                return _db.InsertOrReplace(source);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return -1;
        }

        public bool SetDatabaseConnection(string databasePath)
        {
            if (string.IsNullOrEmpty(databasePath))
            {
                throw new ArgumentNullException("databasePath cannot be null or empty. ex)c:\\MyData.db");
            }

            DbPath = databasePath;
            if (_db != null)
            {
                _db = new SQLiteConnection(DbPath);
            }

            return File.Exists(DbPath);
        }

        public int Update(object item)
        {
            return _db.Update(item);
        }
    }
}
