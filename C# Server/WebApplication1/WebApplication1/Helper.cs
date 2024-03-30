namespace WebApplication1
{
    public class Helpers
    {
        public static string GetRDSConnectionString()
        {

            string dbname = "CCProject";
            string username = "admin";
            string password = "admin123";
            string hostname = "database-1.c90iwec8mj32.us-east-1.rds.amazonaws.com";

            return "Data Source=" + hostname + ";Initial Catalog=" + dbname + ";User ID=" + username + ";Password=" + password + ";";
        }
    }
}
