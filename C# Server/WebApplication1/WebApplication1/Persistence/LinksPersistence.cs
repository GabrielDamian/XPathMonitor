using System.Data;

namespace WebApplication1.Persistence
{
    public interface ILinkService
    {
        int AddLink(int userId, string url, string description);
        bool UpdateLink(int linkId, int userId, string url, string description);
        bool DeleteLink(int linkId, int userId);
        List<Link> GetLinks(int userId);
    }

    public class Link
    {
        public int LinkId { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }


    public class SqlLinksService: ILinkService
    {
        private readonly IDbConnection _dbConnection;

        public SqlLinksService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int AddLink(int userId, string url, string description)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "INSERT INTO links (user_id, url, description, created_at) VALUES (@UserId, @Url, @Description, @CreatedAt); SELECT SCOPE_IDENTITY();";

                var userIdParam = command.CreateParameter();
                userIdParam.ParameterName = "@UserId";
                userIdParam.Value = userId;
                command.Parameters.Add(userIdParam);

                var urlParam = command.CreateParameter();
                urlParam.ParameterName = "@Url";
                urlParam.Value = url;
                command.Parameters.Add(urlParam);

                var descriptionParam = command.CreateParameter();
                descriptionParam.ParameterName = "@Description";
                descriptionParam.Value = description;
                command.Parameters.Add(descriptionParam);

                var createdAtParam = command.CreateParameter();
                createdAtParam.ParameterName = "@CreatedAt";
                createdAtParam.Value = DateTime.UtcNow;
                command.Parameters.Add(createdAtParam);

                int newLinkId = Convert.ToInt32(command.ExecuteScalar());
                _dbConnection.Close();
                return newLinkId;
            }
        }

        public bool UpdateLink(int linkId, int userId, string url, string description)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "UPDATE links SET url = @Url, description = @Description WHERE link_id = @LinkId AND user_id = @UserId";

                var linkIdParam = command.CreateParameter();
                linkIdParam.ParameterName = "@LinkId";
                linkIdParam.Value = linkId;
                command.Parameters.Add(linkIdParam);

                var userIdParam = command.CreateParameter();
                userIdParam.ParameterName = "@UserId";
                userIdParam.Value = userId;
                command.Parameters.Add(userIdParam);

                var urlParam = command.CreateParameter();
                urlParam.ParameterName = "@Url";
                urlParam.Value = url;
                command.Parameters.Add(urlParam);

                var descriptionParam = command.CreateParameter();
                descriptionParam.ParameterName = "@Description";
                descriptionParam.Value = description;
                command.Parameters.Add(descriptionParam);

                int rowsAffected = command.ExecuteNonQuery();
                _dbConnection.Close();
                return rowsAffected > 0;
            }
        }


        public bool DeleteLink(int linkId, int userId)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "DELETE FROM links WHERE link_id = @LinkId AND user_id = @UserId";

                var linkIdParam = command.CreateParameter();
                linkIdParam.ParameterName = "@LinkId";
                linkIdParam.Value = linkId;
                command.Parameters.Add(linkIdParam);

                var userIdParam = command.CreateParameter();
                userIdParam.ParameterName = "@UserId";
                userIdParam.Value = userId;
                command.Parameters.Add(userIdParam);

                int rowsAffected = command.ExecuteNonQuery();
                _dbConnection.Close();
                return rowsAffected > 0;
            }
        }

        public List<Link> GetLinks(int userId)
        {
            List<Link> links = new List<Link>();

            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "SELECT link_id, url, description, created_at FROM links WHERE user_id = @UserId";

                var userIdParam = command.CreateParameter();
                userIdParam.ParameterName = "@UserId";
                userIdParam.Value = userId;
                command.Parameters.Add(userIdParam);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var link = new Link
                        {
                            LinkId = reader.GetInt32(reader.GetOrdinal("link_id")),
                            Url = reader.GetString(reader.GetOrdinal("url")),
                            Description = reader.GetString(reader.GetOrdinal("description")),
                            CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                        };
                        links.Add(link);
                    }
                }

                _dbConnection.Close();
            }

            return links;
        }



    }
}
