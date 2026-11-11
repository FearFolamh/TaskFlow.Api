using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Data;
using TaskFlow.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite("Data Source=taskflow.db");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapProjectEndpoints();
app.Run();



