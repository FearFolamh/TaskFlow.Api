using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Data;
using TaskFlow.Api.Endpoints;
using TaskFlow.Api.Services;



var builder = WebApplication.CreateBuilder(args);


var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"Environment: {environment}");
//builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddScoped<ProjectService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapProjectEndpoints();
app.Run();



