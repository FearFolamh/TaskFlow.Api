
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using TaskFlow.Api.Models;
using TaskFlow.Api.DTOs;
using TaskFlow.Api.Data;
using TaskFlow.Api.Services;


namespace TaskFlow.Api.Endpoints;
public static class ProjectEndpoints
{
    public static void MapProjectEndpoints(this IEndpointRouteBuilder app)
{
    var group = app.MapGroup("/projects").WithTags("Projects");

group.MapGet("/", async (ProjectService projectService, ILogger<Program> logger) => 
{
    logger.LogInformation("Получен запрос на получение всех проектов");
    return Results.Ok(await projectService.GetAllAsync());
}).WithName("GetProjects");

group.MapGet("/{id}", async (int id, ProjectService projectService) => 
{
    var project = await projectService.GetByIdAsync(id);
    if (project == null)
    {   
        return Results.NotFound();
    
    }
    
    
        return Results.Ok(project);
    
}).WithName("GetProjectsId");

group.MapPost("/", async (Project newProject, ProjectService projectService) =>
{

    if (string.IsNullOrWhiteSpace(newProject.Title) || newProject.Title.Length > 100)
    {
        return Results.BadRequest();
    }
    
    
        var createdProject = await projectService.CreateAsync(newProject);
        return Results.Created($"/projects/{createdProject.id}", createdProject);
    
})
.WithName("PostProjects");


group.MapDelete("/{id}", async (int id, ProjectService projectService) =>
{
    var result = await projectService.DeleteAsync(id);
    if (!result)
    {
        return Results.NotFound();
    }
    return Results.NoContent();
    
}).WithName("DeleteProjects");

group.MapPut("/{id}", async (int id, UpdateProjectRequest updatedProject, ProjectService projectService) =>
{
    if (string.IsNullOrWhiteSpace(updatedProject.Title) || updatedProject.Title.Length > 100)
    {
        return Results.BadRequest();
    }
    var project = await projectService.UpdateAsync(id, updatedProject);
    if (project == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(project);
        
    
}).WithName("PutProjects");


group.MapPatch("/{id}", async (int id, UpdateProjectRequest updatedProject, ProjectService projectService, ILogger<Program> logger) =>
{
    if (updatedProject.Title != null && (string.IsNullOrWhiteSpace(updatedProject.Title) || updatedProject.Title.Length > 100))
        {
            return Results.BadRequest();
        }
    var project = await projectService.PatchAsync(id, updatedProject);
    if (project == null)
    {
        return Results.NotFound();
    }
    
    
        

        
        
            logger.LogInformation("Получен Title: {Title}", updatedProject.Title);
            logger.LogInformation("Получен Description: {Description}", updatedProject.Description);
            


            logger.LogInformation("В базе Title: {Title}", project.Title);
            logger.LogInformation("В базе Description: {Description}", project.Description);

            return Results.Ok(project);
        
    
}).WithName("PatchProjects");
}
}