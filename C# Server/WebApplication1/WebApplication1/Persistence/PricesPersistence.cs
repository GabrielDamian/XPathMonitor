using System.Data;

namespace WebApplication1.Persistence
{
    public interface IPriceService
    {
        int AddPrice(int linkId, decimal price, string currency);
        bool UpdatePrice(int priceId, int linkId, decimal price, string currency);
        bool DeletePrice(int priceId, int linkId);
        List<Price> GetPrices(int linkId);
    }

    public class Price
    {
        public int PriceId { get; set; }
        public int LinkId { get; set; }
        public decimal PriceValue { get; set; }
        public string Currency { get; set; }
        public DateTime DateAdded { get; set; }
    }

    public class SqlPriceService : IPriceService
    {
        private readonly IDbConnection _dbConnection;

        public SqlPriceService(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public int AddPrice(int linkId, decimal price, string currency)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "INSERT INTO prices (link_id, price, currency, date_added) VALUES (@LinkId, @Price, @Currency, @DateAdded); SELECT SCOPE_IDENTITY();";

                var linkIdParam = command.CreateParameter();
                linkIdParam.ParameterName = "@LinkId";
                linkIdParam.Value = linkId;
                command.Parameters.Add(linkIdParam);

                var priceParam = command.CreateParameter();
                priceParam.ParameterName = "@Price";
                priceParam.Value = price;
                command.Parameters.Add(priceParam);

                var currencyParam = command.CreateParameter();
                currencyParam.ParameterName = "@Currency";
                currencyParam.Value = currency;
                command.Parameters.Add(currencyParam);

                var dateAddedParam = command.CreateParameter();
                dateAddedParam.ParameterName = "@DateAdded";
                dateAddedParam.Value = DateTime.UtcNow;
                command.Parameters.Add(dateAddedParam);

                int newPriceId = Convert.ToInt32(command.ExecuteScalar());
                _dbConnection.Close();
                return newPriceId;
            }
        }

        public List<Price> GetPrices(int linkId)
        {
            List<Price> prices = new List<Price>();

            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "SELECT price_id, link_id, price, currency, date_added FROM prices WHERE link_id = @LinkId";

                var linkIdParam = command.CreateParameter();
                linkIdParam.ParameterName = "@LinkId";
                linkIdParam.Value = linkId;
                command.Parameters.Add(linkIdParam);

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var price = new Price
                        {
                            PriceId = reader.GetInt32(reader.GetOrdinal("price_id")),
                            LinkId = reader.GetInt32(reader.GetOrdinal("link_id")),
                            PriceValue = reader.GetDecimal(reader.GetOrdinal("price")),
                            Currency = reader.GetString(reader.GetOrdinal("currency")),
                            DateAdded = reader.GetDateTime(reader.GetOrdinal("date_added"))
                        };
                        prices.Add(price);
                    }
                }

                _dbConnection.Close();
            }

            return prices;
        }
        public bool UpdatePrice(int priceId, int linkId, decimal price, string currency)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "UPDATE prices SET price = @Price, currency = @Currency WHERE price_id = @PriceId AND link_id = @LinkId";

                var priceIdParam = command.CreateParameter();
                priceIdParam.ParameterName = "@PriceId";
                priceIdParam.Value = priceId;
                command.Parameters.Add(priceIdParam);

                var linkIdParam = command.CreateParameter();
                linkIdParam.ParameterName = "@LinkId";
                linkIdParam.Value = linkId;
                command.Parameters.Add(linkIdParam);

                var priceParam = command.CreateParameter();
                priceParam.ParameterName = "@Price";
                priceParam.Value = price;
                command.Parameters.Add(priceParam);

                var currencyParam = command.CreateParameter();
                currencyParam.ParameterName = "@Currency";
                currencyParam.Value = currency;
                command.Parameters.Add(currencyParam);

                int rowsAffected = command.ExecuteNonQuery();
                _dbConnection.Close();
                return rowsAffected > 0;
            }
        }
        public bool DeletePrice(int priceId, int linkId)
        {
            using (var command = _dbConnection.CreateCommand())
            {
                _dbConnection.Open();
                command.CommandText = "DELETE FROM prices WHERE price_id = @PriceId AND link_id = @LinkId";

                var priceIdParam = command.CreateParameter();
                priceIdParam.ParameterName = "@PriceId";
                priceIdParam.Value = priceId;
                command.Parameters.Add(priceIdParam);

                var linkIdParam = command.CreateParameter();
                linkIdParam.ParameterName = "@LinkId";
                linkIdParam.Value = linkId;
                command.Parameters.Add(linkIdParam);

                int rowsAffected = command.ExecuteNonQuery();
                _dbConnection.Close();
                return rowsAffected > 0;
            }
        }

    }
}
