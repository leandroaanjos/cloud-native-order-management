using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using OrderManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// MVC Controllers
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infra (DbContext, Repos, etc.)
builder.Services.AddInfrastructure(builder.Configuration);

// Health checks (single registration + chaining)
var postgresConn = builder.Configuration.GetConnectionString("Postgres");

var healthChecks = builder.Services.AddHealthChecks()
    // Liveness: indicates whether the application process is running
    .AddCheck("self", () => HealthCheckResult.Healthy());

if (!string.IsNullOrWhiteSpace(postgresConn))
{
    // Readiness: verifies external dependencies (PostgreSQL) before accepting traffic
    healthChecks.AddNpgSql(postgresConn, name: "postgres");
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Map controllers
app.MapControllers();

// Health endpoints
app.MapHealthChecks("/health/live", new HealthCheckOptions
{
    Predicate = r => r.Name == "self"
});

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
    Predicate = r => r.Name == "postgres"
});

app.MapHealthChecks("/health");

app.Run();