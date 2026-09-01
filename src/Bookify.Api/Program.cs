using Bookify.Api.Extensions;
using Bookify.Application;
using Bookify.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(
    (context, configuration) => configuration.ReadFrom.Configuration(context.Configuration)
);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication(); // Adds the application layer
builder.Services.AddInfrastructure(builder.Configuration); // Adds the Infrastructure layer

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations();
    // Do this once to seed data then comment it out
    //app.SeedData();
}

app.UseHttpsRedirection();

app.UseRequestContextLogging();

app.UseSerilogRequestLogging();

app.UseCustomExceptionHandling();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
