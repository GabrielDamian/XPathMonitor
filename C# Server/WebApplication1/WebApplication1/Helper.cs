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
            string configFilePath = "config.json";
            string json = File.ReadAllText(configFilePath);
            Config config = JsonSerializer.Deserialize<Config>(json);

            string dbname = "CCProject";
            string username = "admin";
            string password = config.RDS_PASSWORD;
            string hostname = config.RDS_HOSTNAME;

            return $"Data Source={hostname};Initial Catalog={dbname};User ID={username};Password={password};";
        }
    }
}
