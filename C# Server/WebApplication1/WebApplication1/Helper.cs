using System;
using System.IO;
using System.Text.Json;

namespace WebApplication1
{
    public class Config
    {
        public string RDS_PASSWORD { get; set; }
        public string RDS_HOSTNAME { get; set; }
    }

    public class Helpers
    {
        public static string GetRDSConnectionString()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            var configuration = builder.Build();

            string dbname = "CCProject";
            string username = "admin";
            string password = configuration["RDS_PASSWORD"];
            string hostname = configuration["RDS_HOSTNAME"];

            return $"Data Source={hostname};Initial Catalog={dbname};User ID={username};Password={password};";
        }
    }
}
