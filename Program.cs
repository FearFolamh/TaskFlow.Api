using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Data;
using TaskFlow.Api.Endpoints;
using TaskFlow.Api.Services;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=taskflow.db");
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



