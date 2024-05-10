using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace localStorage
{
    internal class LocalStorage : ILocalStorage
    {
        private readonly string connectionString;
        public LocalStorage(string localStoragePath)
        {
            connectionString = $"Data Source={localStoragePath};";
        }


        

        public async Task<string?> GetItemAsync(string key)
        {
            using DbConnection dbConnection = new SQLiteConnection(connectionString);
            dbConnection.Open();

            string sql = $"SELECT * FROM localDb WHERE Title = '{key}'";

            IEnumerable<dynamic> data = await dbConnection.QueryAsync(sql);


            return data.FirstOrDefault()?.Title;


        }

        public async Task SetItemAsync(string key, string value, DateTime? expired = null)
        {
            if (expired == null)
            {
                expired = DateTime.MaxValue;
            }

            using DbConnection db = new SQLiteConnection(connectionString);
            db.Open();

            string sql = $"INSERT INTO localDb (Title,Value,Expired) VALUES('{key}','{value}','{expired:yyyy-MM-dd HH:mm:ss.fffffff}')";

            await db.ExecuteAsync(sql);

            db.Close();
        }
    }
}
