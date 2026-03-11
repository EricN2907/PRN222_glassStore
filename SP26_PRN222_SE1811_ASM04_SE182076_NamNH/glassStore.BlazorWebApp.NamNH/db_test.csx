using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;

var config = new ConfigurationBuilder()
    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true)
    .Build();

string connStr = config.GetConnectionString("DefaultConnection") ?? "Data Source=NEM\\SQLEXPRESS;Initial Catalog=glass_Store;Persist Security Info=True;User ID=sa;Password=12345;TrustServerCertificate=True";

Console.WriteLine($"Connection String: {connStr}");

try {
    using var conn = new SqlConnection(connStr);
    conn.Open();
    using var cmd = conn.CreateCommand();
    cmd.CommandText = "SELECT COUNT(*) FROM Orders_NamNH";
    var result = cmd.ExecuteScalar();
    Console.WriteLine($"Count in DB: {result}");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
