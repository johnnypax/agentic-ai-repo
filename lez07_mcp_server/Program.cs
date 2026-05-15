var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<CompanyUserRepository>();

builder.Services
    .AddMcpServer()
    .WithHttpTransport()
    .WithToolsFromAssembly();

var app = builder.Build();

app.MapGet("/health", () => "Company directory MCP is healthy!");

app.MapMcp();

app.Run();
