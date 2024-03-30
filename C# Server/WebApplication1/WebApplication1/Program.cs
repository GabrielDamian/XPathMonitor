using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

// services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var awsSqlConnectionString = Helpers.GetRDSConnectionString();
builder.Services.AddSingleton<IDbConnection>(_ =>
{
    var connection = new SqlConnection(awsSqlConnectionString);
    connection.ConnectionString += ";Encrypt=true;TrustServerCertificate=true";
    return connection;
});

builder.Services.AddScoped<IDataService, SqlDataService>();

// service IConfiguration
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

// JWT key
var jwtSecret = builder.Configuration["Jwt:Secret"];
var key = Encoding.ASCII.GetBytes(jwtSecret);

builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication(); // auth middleware
app.UseAuthorization();
app.MapControllers();

app.Run();
