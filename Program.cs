using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

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


Project[] projects = new Project[]   
{
   

};



app.MapGet("/projects", async (AppDbContext db) => Results.Ok(await db.Projects.ToListAsync())).WithName("GetProjects");

app.MapGet("/projects/{id}", async (int id, AppDbContext db) => 
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
    {   
        return Results.NotFound();
    
    }
    else
    {   
        return Results.Ok(project);
    }
}).WithName("GetProjectsId");

app.MapPost("/projects", async (Project newProject, AppDbContext db) =>
{

    if (string.IsNullOrWhiteSpace(newProject.Title) || newProject.Title.Length > 100)
    {
        return Results.BadRequest();
    }
    else
    {
        db.Projects.Add(newProject);
        await db.SaveChangesAsync();
        return Results.Created($"/projects/{newProject.id}", newProject);
    }
})
.WithName("PostProjects");


app.MapDelete("/projects/{id}", async (int id, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
    {
        return Results.NotFound();
    }
    else
    {
        db.Projects.Remove(project);
        await db.SaveChangesAsync();
        return Results.NoContent();
    }
}).WithName("DeleteProjects");

app.MapPut("/projects/{id}", async (int id, Project updatedProject, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
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
            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;
            await db.SaveChangesAsync();
            return Results.Ok(project);
        }
    }
}).WithName("PutProjects");


app.MapPatch("/projects/{id}", async (int id, UpdateProjectRequest updatedProject, AppDbContext db) =>
{
    var project = await db.Projects.FindAsync(id);
    if (project == null)
    {
        return Results.NotFound();
    }
    else
    {
        
        if (updatedProject.Title != null && (string.IsNullOrWhiteSpace(updatedProject.Title) || updatedProject.Title.Length > 100))
        {
            return Results.BadRequest();
        }
        else
        {   
            Console.WriteLine($"Title: {updatedProject.Title ?? "NULL"}");
            Console.WriteLine($"Description: {updatedProject.Description ?? "NULL"}");
            if (updatedProject.Title != null)
            {
                
                project.Title = updatedProject.Title;
            }
            if (updatedProject.Description != null)
            {
                project.Description = updatedProject.Description;
            }
            Console.WriteLine($"Получен Title: [{updatedProject.Title ?? "NULL"}]");
            Console.WriteLine($"Получен Description: [{updatedProject.Description ?? "NULL"}]");

            Console.WriteLine($"В базе Title: [{project.Title ?? "NULL"}]");
            Console.WriteLine($"В базе Description: [{project.Description ?? "NULL"}]");
            await db.SaveChangesAsync();
            return Results.Ok(project);
        }
    }
}).WithName("PatchProjects");

app.Run();

public class Project
{
    public int id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
}

public class UpdateProjectRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
}