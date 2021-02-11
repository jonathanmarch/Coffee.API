using System;
using System.IO;
using System.Linq;
using Coffee.API;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.SqlServer.Management.Common;
using Microsoft.SqlServer.Management.Smo;

namespace CoffeeApi.Tests
{
    public class WebApplicationWithDatabaseFixture : IDisposable
    {
        private static Random random = new Random();
        private const string ConnectionStringKey = "ConnectionStrings:DefaultConnectionString";

        public readonly WebApplicationFactory<Startup> factory;
        public readonly string originalConnectionString;
        public readonly string randomDbName;

        public WebApplicationWithDatabaseFixture()
        {
            var configuration = GetIConfigurationRoot();
            randomDbName = $"Coffee-Api-Test-{RandomString(10).ToLower()}";
            
            originalConnectionString = configuration[ConnectionStringKey];

            configuration[ConnectionStringKey] = configuration[ConnectionStringKey].Replace("master", randomDbName);

            CreateAndSeedTestDatabase();
            
            this.factory = new WebApplicationFactory<Startup>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddConfiguration(configuration);
                });
            });
        }

        public void Dispose()
        {
            DropTestDatabase();
            factory.Dispose();
        }

        private void CreateAndSeedTestDatabase()
        {
            var createDatabaseSqlText = File.ReadAllText("Database/Coffee-Api-With-Seed-Data.sql");

            createDatabaseSqlText = createDatabaseSqlText.Replace("Coffee-Api", randomDbName);

            var server =
                new Server(new ServerConnection(new SqlConnection(originalConnectionString)));
            
            server.ConnectionContext.ExecuteNonQuery(createDatabaseSqlText);
            server.ConnectionContext.Disconnect();
        }
        
        private void DropTestDatabase()
        {
            var dropDatabaseSqlText = $"DROP DATABASE [{randomDbName}];";
            
            var server =
                new Server(new ServerConnection(new SqlConnection(originalConnectionString)));

            server.ConnectionContext.ExecuteNonQuery(dropDatabaseSqlText);
            server.ConnectionContext.Disconnect();
        }
        
        private static IConfigurationRoot GetIConfigurationRoot()
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();
        }

        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
