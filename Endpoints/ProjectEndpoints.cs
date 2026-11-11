
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Data;

namespace TaskFlow.Api.Endpoints;
public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
{
    var group = app.MapGroup("/projects").WithTags("Projects");

group.MapGet("/", async (AppDbContext db) => Results.Ok(await db.Projects.ToListAsync())).WithName("GetProjects");

group.MapGet("/{id}", async (int id, AppDbContext db) => 
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
    {   
        return Results.NotFound();
    
    }
    
    {   
        return Results.Ok(project);
    }
}).WithName("GetProjectsId");

group.MapPost("/", async (Project newProject, AppDbContext db) =>
{

    if (string.IsNullOrWhiteSpace(newProject.Title) || newProject.Title.Length > 100)
    {
        return Results.BadRequest();
    }
    
    {
        db.Projects.Add(newProject);
        await db.SaveChangesAsync();
        return Results.Created($"/projects/{newProject.id}", newProject);
    }
})
.WithName("PostProjects");


group.MapDelete("/{id}", async (int id, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
    {
        return Results.NotFound();
    }
    
    {
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
}).WithName("DeleteProjects");

group.MapPut("/{id}", async (int id, Project updatedProject, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
    {
        return Results.NotFound();
    }
    
    {
        if (string.IsNullOrWhiteSpace(updatedProject.Title) || updatedProject.Title.Length > 100)
        {
            return Results.BadRequest();
        }
        
        {
            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;
            await db.SaveChangesAsync();
            return Results.Ok(project);
        }
    }
}).WithName("PutProjects");


group.MapPatch("/{id}", async (int id, UpdateProjectRequest updatedProject, AppDbContext db,ILogger logger) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
    {
        return Results.NotFound();
    }
    
    {
        
        if (updatedProject.Title != null && (string.IsNullOrWhiteSpace(updatedProject.Title) || updatedProject.Title.Length > 100))
        {
            return Results.BadRequest();
        }
        
        {   
            logger.LogInformation("Получен Title: {Title}", updatedProject.Title);
            logger.LogInformation("Получен Description: {Description}", updatedProject.Description);
            if (updatedProject.Title != null)
            {
                
                project.Title = updatedProject.Title;
            }
            if (updatedProject.Description != null)
            {
                project.Description = updatedProject.Description;
            }


            logger.LogInformation("В базе Title: {Title}", project.Title);
            logger.LogInformation("В базе Description: {Description}", project.Description);
            await db.SaveChangesAsync();
            return Results.Ok(project);
        }
    }
}).WithName("PatchProjects");
}
}