using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Text;
using WebApplication1;
using WebApplication1.Persistence;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

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
builder.Services.AddScoped<ILinkService, SqlLinksService>();
builder.Services.AddScoped<IPriceService, SqlPriceService>();

builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

var jwtSecret = builder.Configuration["JWT_TOKEN"];
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


app.UseCors("AllowAllOrigins"); // middleware CORS
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();