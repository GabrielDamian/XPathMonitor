using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Text;
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

        public static int GetUserIdFromToken(HttpContext httpContext, IConfiguration configuration)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token == null)
            {
                throw new ApplicationException("Tokenul lipsește.");
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(configuration["JWT_TOKEN"]);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                }, out var validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userIdClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == "userId");

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    throw new ApplicationException("ID-ul utilizatorului lipsește sau nu este valid.");
                }

                return userId;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Tokenul nu este valid: " + ex.Message);
            }
        }

    }
}
