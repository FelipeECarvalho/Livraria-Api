using Livraria;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

Configuration.ConnectionString = app.Configuration.GetValue<string>("ConnectionString");

app.Run();
