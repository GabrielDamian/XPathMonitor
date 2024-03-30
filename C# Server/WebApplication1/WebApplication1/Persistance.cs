using Microsoft.Data.SqlClient;
using System.Data;

namespace WebApplication1
{
    public interface IDataService
    {
        bool UserExists(string username);
        bool CreateUser(string username, string password);
        bool ValidateUser(string username, string password);
    }

    public class SqlDataService : IDataService
    {
        private readonly IDbConnection _dbConnection;

        public SqlDataService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public bool UserExists(string username)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "SELECT COUNT(*) FROM USERS WHERE username = @Username";

                var usernameParam = command.CreateParameter();
                usernameParam.ParameterName = "@Username";
                usernameParam.Value = username;
                command.Parameters.Add(usernameParam);

                int count = (int)command.ExecuteScalar();
                _dbConnection.Close();
                return count > 0;
            }
        }

        public bool CreateUser(string username, string password)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "INSERT INTO USERS (username, password_hash) VALUES (@Username, @Password)";

                var usernameParam = command.CreateParameter();
                usernameParam.ParameterName = "@Username";
                usernameParam.Value = username;
                command.Parameters.Add(usernameParam);

                var passwordParam = command.CreateParameter();
                passwordParam.ParameterName = "@Password";
                passwordParam.Value = password;
                command.Parameters.Add(passwordParam);

                int rowsAffected = command.ExecuteNonQuery();
                _dbConnection.Close();
                return rowsAffected > 0;
            }
        }

        public bool ValidateUser(string username, string password)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "SELECT COUNT(*) FROM USERS WHERE username = @Username AND password_hash = @Password";

                var usernameParam = command.CreateParameter();
                usernameParam.ParameterName = "@Username";
                usernameParam.Value = username;
                command.Parameters.Add(usernameParam);

                var passwordParam = command.CreateParameter();
                passwordParam.ParameterName = "@Password";
                passwordParam.Value = password;
                command.Parameters.Add(passwordParam);

                int count = (int)command.ExecuteScalar();
                _dbConnection.Close();
                return count > 0;
            }
        }
    }
}
