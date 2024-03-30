using Microsoft.Data.SqlClient;
using System.Data;
using WebApplication1;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuring AWS SQL Database connection
var awsSqlConnectionString = Helpers.GetRDSConnectionString();
Console.WriteLine(awsSqlConnectionString);

builder.Services.AddSingleton<IDbConnection>(_ =>{
    var connection = new SqlConnection(awsSqlConnectionString);
    connection.ConnectionString += ";Encrypt=true;TrustServerCertificate=true";
    return connection;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Check and create USERS table
using (var scope = app.Services.CreateScope())
{
    var dbConnection = scope.ServiceProvider.GetRequiredService<IDbConnection>();
    dbConnection.Open();

    // Check if USERS table exists, if not, create it
    var command = dbConnection.CreateCommand();
    command.CommandText = @"
        IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'USERS')
        BEGIN
            CREATE TABLE USERS (
                Username NVARCHAR(100),
                Password NVARCHAR(100)
            )
        END";
    command.ExecuteNonQuery();

    dbConnection.Close();
}

app.Run();
