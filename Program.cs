using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


Project[] projects = new Project[]   
{
    new Project(1, "Project 1", "Description of Project 1"),
    new Project(2, "Project 2", "Description of Project 2"),

};



app.MapGet("/projects", () => Results.Ok(projects)).WithName("GetProjects");
/*{  
    return projects;
})
.WithName("GetProjects");
*/

app.MapGet("/projects/{id}", (int id) => 
{
    var project = projects.FirstOrDefault(p => p.id == id);
    if (project == null)
    {   
        return Results.NotFound();
    
    }
    else
    {   
        return Results.Ok(project);
    }
}).WithName("GetProjectsId");

app.MapPost("/projects", (Project newProject) =>
{

    if (string.IsNullOrWhiteSpace(newProject.Title) || newProject.Title.Length > 100)
    {
        return Results.BadRequest();
    }
    else
    {
        var project = new Project(projects.Select(p => p.id).DefaultIfEmpty(0).Max() + 1, newProject.Title, newProject.Description);
        projects = projects.Append(project).ToArray();
        return Results.Created($"/projects/{project.id}", project);
    }
})
.WithName("PostProjects");


app.MapDelete("/projects/{id}", (int id) =>
{
    var project = projects.FirstOrDefault(p => p.id == id);
    if (project == null)
    {
        return Results.NotFound();
    }
    else
    {
        projects = projects.Where(p => p.id != id).ToArray();
        return Results.NoContent();
    }
}).WithName("DeleteProjects");

app.MapPut("/projects/{id}", (int id, Project updatedProject) =>
{
    var project = projects.FirstOrDefault(p => p.id == id);
    if (project == null)
    {
        return Results.NotFound();
    }
    else
    {
        if (string.IsNullOrWhiteSpace(updatedProject.Title) || updatedProject.Title.Length > 100)
        {
            return Results.BadRequest();
        }
        else
        {
            var updated = new Project(id, updatedProject.Title, updatedProject.Description);
            projects = projects.Select(p => p.id == id ? updated : p).ToArray();
            return Results.Ok(updated);
        }
    }
}).WithName("PutProjects");


app.MapPatch("/projects/{id}", (int id, Project updatedProject) =>
{
    var project = projects.FirstOrDefault(p => p.id == id);
    if (project == null)
    {
        return Results.NotFound();
    }
    else
    {
        var updated = new Project(id, updatedProject.Title ?? project.Title, updatedProject.Description ?? project.Description);
        projects = projects.Select(p => p.id == id ? updated : p).ToArray();
        return Results.Ok(updated);
    }
}).WithName("PatchProjects");

app.Run();

record Project(int id, string? Title, string? Description)
{
   
}
