using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace localStorage
{
    public static class Extensions
    {
        public static IServiceCollection AddLocationStorage(this IServiceCollection services, string stroagePath)
        {

            services.AddTransient<ILocalStorage, LocalStorage>(s =>
            {
                return new LocalStorage(stroagePath);
            });


            return services;
        }



        public static IApplicationBuilder UseLocalStorage(this IApplicationBuilder app, string stroagePath)
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), stroagePath);

            string connectionString = $"Data Source={stroagePath}.db;";

            if (!File.Exists(path))
            {
                var fileStream = File.Create(path);

                fileStream.Close();



                using (DbConnection db = new SQLiteConnection(connectionString))
                {
                    db.Open();

                    db.Execute(@"CREATE TABLE IF NOT EXISTS localDb (
                        Title TEXT NOT NULL UNIQUE PRIMARY KEY,
                        Value TEXT NOT NULL,
                        Expired DATETIME2 NOT NULL
                    )");

                    db.Close();
                }
            }
            else
            {
                using (DbConnection db = new SQLiteConnection(connectionString))
                {
                    db.Open();

                    db.Execute(@"DELETE FROM localDb");


                    db.Close();
                }
            }







            return app;
        }
    }
}
