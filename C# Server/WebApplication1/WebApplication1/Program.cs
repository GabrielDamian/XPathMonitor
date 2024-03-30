using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Data;
using Microsoft.Data.SqlClient;
using WebApplication1;

var builder = WebApplication.CreateBuilder(args);

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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
